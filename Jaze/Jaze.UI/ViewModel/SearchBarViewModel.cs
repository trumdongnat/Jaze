using System;
using System.Threading.Tasks;
using System.Windows;
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
        private DictionaryType _dictionaryType;

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
            _isSearching = false;
            SearchAsync(key, _dictionaryType, _searchOption);
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
                        _messenger.Send(new SearchResultMessage(_javiService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.HanViet:
                        _messenger.Send(new SearchResultMessage(_hanvietService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.Kanji:
                        _messenger.Send(new SearchResultMessage(_kanjiService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.ViJa:
                        _messenger.Send(new SearchResultMessage(_vijaService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.Grammar:
                        _messenger.Send(new SearchResultMessage(_grammarService.Search(new SearchArgs(key, searchOption))));
                        break;

                    case DictionaryType.JaEn:
                        _messenger.Send(new SearchResultMessage(_jaenService.Search(new SearchArgs(key, searchOption))));
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
            MessageBox.Show(option.ToString());
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
        }

        #endregion ----- Contructor -----
    }
}