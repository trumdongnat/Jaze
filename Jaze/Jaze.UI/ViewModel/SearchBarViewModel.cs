using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Services;

namespace Jaze.UI.ViewModel
{
    public class SearchBarViewModel : ViewModelBase
    {
        #region ----- Properties -----

        //private DictionaryType _dictionaryType = DictionaryType.HanViet;

        #endregion ----- Properties -----

        #region ----- List Dictionary -----

        /// <summary>
        /// The <see cref="ListDictionary" /> property's name.
        /// </summary>
        public const string ListDictionaryPropertyName = "ListDictionary";

        private List<Dictionary> _listDictionary = null;

        /// <summary>
        /// Sets and gets the ListDictionary property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public List<Dictionary> ListDictionary
        {
            get
            {
                return _listDictionary;
            }
            set
            {
                Set(ListDictionaryPropertyName, ref _listDictionary, value);
            }
        }

        #endregion ----- List Dictionary -----

        #region ----- Selected Dictionary -----

        /// <summary>
        /// The <see cref="SelectedDictionary" /> property's name.
        /// </summary>
        public const string SelectedDictionaryPropertyName = "SelectedDictionary";

        private Dictionary _selectedDictionary;

        /// <summary>
        /// Sets and gets the SelectedDictionary property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Dictionary SelectedDictionary
        {
            get
            {
                return _selectedDictionary;
            }
            set
            {
                Set(SelectedDictionaryPropertyName, ref _selectedDictionary, value);
            }
        }

        #endregion ----- Selected Dictionary -----

        #region ----- Services -----

        private IMessenger _messenger;
        private ISearchService<GrammarModel> _grammarService;
        private ISearchService<HanVietModel> _hanvietService;
        private ISearchService<JaenModel> _jaenService;
        private ISearchService<JaviModel> _javiService;
        private ISearchService<KanjiModel> _kanjiService;
        private ISearchService<VijaModel> _vijaService;

        #endregion ----- Services -----

        #region ----- Search Command -----

        private bool _isSearching;
        private RelayCommand<string> _searchCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand<string> SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand<string>(
                           ExecuteSearchCommand,
                           CanExecuteSearchCommand));
            }
        }

        private void ExecuteSearchCommand(string key)
        {
            _isSearching = true;
            var dictionaryType = SelectedDictionary.Type;
            _messenger.Send(new SearchMessage(SearchStates.Searching, dictionaryType, null));
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
                        _messenger.Send(new SearchMessage(SearchStates.Success, dictionaryType, _javiService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.HanViet:
                        _messenger.Send(new SearchMessage(SearchStates.Success, dictionaryType, _hanvietService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.Kanji:
                        _messenger.Send(new SearchMessage(SearchStates.Success, dictionaryType, _kanjiService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.ViJa:
                        _messenger.Send(new SearchMessage(SearchStates.Success, dictionaryType, _vijaService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.Grammar:
                        _messenger.Send(new SearchMessage(SearchStates.Success, dictionaryType, _grammarService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.JaEn:
                        _messenger.Send(new SearchMessage(SearchStates.Success, dictionaryType, _jaenService.Search(new SearchArgs(key, searchOption))));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(dictionaryType), dictionaryType, null);
                }
                _isSearching = false;
            });
        }

        #endregion ----- Search Command -----

        #region ----- Change Search Option -----

        private RelayCommand<SearchOption> _searchOptionChange;
        private SearchOption _searchOption;

        /// <summary>
        /// Gets the ChangeSearchOptionCommand.
        /// </summary>
        public RelayCommand<SearchOption> ChangeSearchOptionCommand
        {
            get
            {
                return _searchOptionChange
                    ?? (_searchOptionChange = new RelayCommand<SearchOption>(ExecuteMyCommand));
            }
        }

        private void ExecuteMyCommand(SearchOption option)
        {
            _searchOption = option;
        }

        #endregion ----- Change Search Option -----

        #region ----- Contructor -----

        public SearchBarViewModel(IMessenger messenger, ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService)
        {
            _messenger = messenger;
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