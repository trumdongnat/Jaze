using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Repository;
using Jaze.UI.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

namespace Jaze.UI.ViewModel
{
    public class SearchBarViewModel : ViewModelBase
    {
        private bool _isDislayingMatchedResult;

        #region ----- Services -----

        private readonly IEventAggregator _eventAggregator;

        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IDialogService _dialogService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private List<Dictionary> _listDictionary = null;

        public List<Dictionary> ListDictionary
        {
            get => _listDictionary;
            set => SetProperty(ref _listDictionary, value);
        }

        private Dictionary _selectedDictionary;

        public Dictionary SelectedDictionary
        {
            get => _selectedDictionary;
            set => SetProperty(ref _selectedDictionary, value, OnSelectedDictionaryChanged);
        }

        private void OnSelectedDictionaryChanged()
        {
            if (_isDislayingMatchedResult)
            {
                ExecuteSearchCommand(SearchKey);
            }
        }

        private string _searchKey = string.Empty;

        public string SearchKey
        {
            get => _searchKey;
            set => SetProperty(ref _searchKey, value, OnSearchKeyChanged);
        }

        private void OnSearchKeyChanged()
        {
            _isDislayingMatchedResult = false;
        }

        #endregion ----- Properties -----

        #region ----- Commands -----

        private bool _isSearching;
        private DelegateCommand<string> _searchCommand;

        public DelegateCommand<string> SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand<string>(
                                                            ExecuteSearchCommand,
                                                            CanExecuteSearchCommand));

        private async void ExecuteSearchCommand(string key)
        {
            _isDislayingMatchedResult = false;
            _isSearching = true;
            var dictionaryType = SelectedDictionary.Type;
            var pubSub = _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>();
            pubSub.Publish(new SearchMessage(SearchStates.Searching, dictionaryType, null));
            //SearchAsync(key, dictionaryType, _searchOption);
            var result = await _dictionaryRepository.SearchAsync(new SearchArgs(key, _searchOption, dictionaryType));
            pubSub.Publish(new SearchMessage(SearchStates.Success, dictionaryType, result));
            _isSearching = false;
            _isDislayingMatchedResult = true;
        }

        private bool CanExecuteSearchCommand(string key)
        {
            return !_isSearching;
        }

        private DelegateCommand _pasteToSearchCommand;

        public DelegateCommand PasteToSearchCommand
        {
            get
            {
                return _pasteToSearchCommand
                       ?? (_pasteToSearchCommand = new DelegateCommand(
                           () =>
                           {
                               var key = Clipboard.GetText();
                               if (SearchCommand.CanExecute(key))
                               {
                                   SearchKey = key;
                                   SearchCommand.Execute(key);
                               }
                           }));
            }
        }

        private DelegateCommand _showKanjiPartCommand;

        public DelegateCommand ShowKanjiPartCommand => _showKanjiPartCommand ?? (_showKanjiPartCommand = new DelegateCommand(
                                                           ExecuteShowKanjiPartCommand,
                                                           CanExecuteShowKanjiPartCommand));

        private void ExecuteShowKanjiPartCommand()
        {
            _dialogService.ShowKanjiPartDialog(new List<string>());
        }

        private bool CanExecuteShowKanjiPartCommand()
        {
            return true;
        }

        private DelegateCommand<SearchOption?> _searchOptionChange;
        private SearchOption _searchOption;

        public DelegateCommand<SearchOption?> ChangeSearchOptionCommand => _searchOptionChange
                                                                           ?? (_searchOptionChange = new DelegateCommand<SearchOption?>(ExecuteChangeSearchOptionCommand));

        private void ExecuteChangeSearchOptionCommand(SearchOption? option)
        {
            if (option != null)
            {
                _searchOption = (SearchOption)option;
                if (_isDislayingMatchedResult)
                {
                    ExecuteSearchCommand(SearchKey);
                }
            }
        }

        #endregion ----- Commands -----

        #region ----- Contructor -----

        public SearchBarViewModel(IEventAggregator eventAggregator, IDictionaryRepository dictionaryRepository, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _dictionaryRepository = dictionaryRepository;
            _dialogService = dialogService;
            ListDictionary = _dictionaryRepository.GetDictionarys();
            SelectedDictionary = ListDictionary[0];
        }

        #endregion ----- Contructor -----
    }
}