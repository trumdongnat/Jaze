using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        #region ----- Services -----

        private IMessenger _messenger;
        //private ISearchService<GrammarModel> _grammarService;
        //private ISearchService<HanVietModel> _hanvietService;
        //private ISearchService<JaenModel> _jaenService;
        //private ISearchService<JaviModel> _javiService;
        //private ISearchService<KanjiModel> _kanjiService;
        //private ISearchService<VijaModel> _vijaService;

        #endregion ----- Services -----

        #region ----- IsSearching -----

        /// <summary>
        /// The <see cref="IsSearching" /> property's name.
        /// </summary>
        public const string IsSearchingPropertyName = "IsSearching";

        private bool _isSearching = false;

        /// <summary>
        /// Sets and gets the IsSearching property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsSearching
        {
            get
            {
                return _isSearching;
            }
            set
            {
                Set(IsSearchingPropertyName, ref _isSearching, value);
            }
        }

        #endregion ----- IsSearching -----

        #region ----- ListItems -----

        /// <summary>
        /// The <see cref="ListItems" /> property's name.
        /// </summary>
        public const string ListItemsPropertyName = "ListItems";

        private object _listItem = new List<object>();

        /// <summary>
        /// Sets and gets the ListItems property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public object ListItems
        {
            get
            {
                return _listItem;
            }
            set
            {
                Set(ListItemsPropertyName, ref _listItem, value);
            }
        }

        #endregion ----- ListItems -----

        #region ----- Current Item -----

        /// <summary>
        /// The <see cref="SelectedItem" /> property's name.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";

        private object _selectedItem = null;

        /// <summary>
        /// Sets and gets the SelectedItem property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                Set(SelectedItemPropertyName, ref _selectedItem, value);
                _messenger.Send(new ShowItemMessage(value));
            }
        }

        #endregion ----- Current Item -----

        #region ----- Constructor -----

        public SearchResultViewModel(IMessenger messenger)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _messenger.Register<SearchMessage>(this, searchMessage => ProcessSearchMessage(searchMessage));
        }

        #endregion ----- Constructor -----

        #region ----- Process Event Messages -----

        private void ProcessSearchMessage(SearchMessage searchMessage)
        {
            if (searchMessage != null)
            {
                switch (searchMessage.SearchState)
                {
                    case SearchStates.Searching:
                        IsSearching = true;
                        break;

                    case SearchStates.FailedToSearch:
                        ListItems = new List<object>();
                        IsSearching = false;
                        break;

                    case SearchStates.Success:
                        ListItems = searchMessage.Result;
                        IsSearching = false;
                        break;
                }
            }
        }

        #endregion ----- Process Event Messages -----
    }
}