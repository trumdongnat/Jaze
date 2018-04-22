using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Prism.Commands;
using Prism.Events;

namespace Jaze.UI.ViewModel
{
    public class SearchBarViewModel : ViewModelBase
    {
        #region ----- List Dictionary -----

        private List<Dictionary> _listDictionary = null;

        public List<Dictionary> ListDictionary
        {
            get => _listDictionary;
            set => SetProperty(ref _listDictionary, value);
        }

        #endregion ----- List Dictionary -----

        #region ----- Selected Dictionary -----

        private Dictionary _selectedDictionary;

        public Dictionary SelectedDictionary
        {
            get => _selectedDictionary;
            set => SetProperty(ref _selectedDictionary, value);
        }

        #endregion ----- Selected Dictionary -----

        #region ----- Services -----

        private readonly IEventAggregator _eventAggregator;
        private readonly ISearchService<GrammarModel> _grammarService;
        private readonly ISearchService<HanVietModel> _hanvietService;
        private readonly ISearchService<JaenModel> _jaenService;
        private readonly ISearchService<JaviModel> _javiService;
        private readonly ISearchService<KanjiModel> _kanjiService;
        private readonly ISearchService<VijaModel> _vijaService;

        #endregion ----- Services -----

        #region ----- Search Key -----

        private string _searchKey = string.Empty;

        public string SearchKey
        {
            get => _searchKey;
            set => SetProperty(ref _searchKey, value);
        }

        #endregion ----- Search Key -----

        #region ----- Search Command -----

        private bool _isSearching;
        private DelegateCommand<string> _searchCommand;

        public DelegateCommand<string> SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand<string>(
                                                            ExecuteSearchCommand,
                                                            CanExecuteSearchCommand));

        private void ExecuteSearchCommand(string key)
        {
            _isSearching = true;
            var dictionaryType = SelectedDictionary.Type;
            _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Publish(new SearchMessage(SearchStates.Searching, dictionaryType, null));
            SearchAsync(key, dictionaryType, _searchOption);
        }

        private bool CanExecuteSearchCommand(string key)
        {
            return !_isSearching;
        }

        private async void SearchAsync(string key, DictionaryType dictionaryType, SearchOption searchOption)
        {
            await Task.Run(() =>
            {
                switch (dictionaryType)
                {
                    case DictionaryType.JaVi:
                        _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Publish(new SearchMessage(SearchStates.Success, dictionaryType, _javiService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.HanViet:
                        _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Publish(new SearchMessage(SearchStates.Success, dictionaryType, _hanvietService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.Kanji:
                        _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Publish(new SearchMessage(SearchStates.Success, dictionaryType, _kanjiService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.ViJa:
                        _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Publish(new SearchMessage(SearchStates.Success, dictionaryType, _vijaService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.Grammar:
                        _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Publish(new SearchMessage(SearchStates.Success, dictionaryType, _grammarService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.JaEn:
                        _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Publish(new SearchMessage(SearchStates.Success, dictionaryType, _jaenService.Search(new SearchArgs(key, searchOption))));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(dictionaryType), dictionaryType, null);
                }
                _isSearching = false;
            });
        }

        #endregion ----- Search Command -----

        #region ----- Paste To Search -----

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

        #endregion ----- Paste To Search -----

        #region ----- Show Kanji Part Command -----

        private DelegateCommand _showKanjiPartCommand;

        public DelegateCommand ShowKanjiPartCommand => _showKanjiPartCommand ?? (_showKanjiPartCommand = new DelegateCommand(
                                                           ExecuteShowKanjiPartCommand,
                                                           CanExecuteShowKanjiPartCommand));

        private void ExecuteShowKanjiPartCommand()
        {
            _eventAggregator.GetEvent<PubSubEvent<ShowPartsMessage>>().Publish(new ShowPartsMessage(new List<string>()));
        }

        private bool CanExecuteShowKanjiPartCommand()
        {
            return true;
        }

        #endregion ----- Show Kanji Part Command -----

        #region ----- Change Search Option -----

        private DelegateCommand<SearchOption?> _searchOptionChange;
        private SearchOption _searchOption;

        public DelegateCommand<SearchOption?> ChangeSearchOptionCommand => _searchOptionChange
                                                                          ?? (_searchOptionChange = new DelegateCommand<SearchOption?>(ExecuteMyCommand));

        private void ExecuteMyCommand(SearchOption? option)
        {
            if (option != null)
            {
                _searchOption = (SearchOption)option;
            }
        }

        #endregion ----- Change Search Option -----

        #region ----- Contructor -----

        public SearchBarViewModel(IEventAggregator eventAggregator, ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService)
        {
            _eventAggregator = eventAggregator;
            _grammarService = grammarService;
            _hanvietService = hanvietService;
            _jaenService = jaenService;
            _javiService = javiService;
            _kanjiService = kanjiService;
            _vijaService = vijaService;
            ListDictionary = Dictionary.GetDictionarys();
            SelectedDictionary = ListDictionary[0];
        }

        #endregion ----- Contructor -----
    }
}