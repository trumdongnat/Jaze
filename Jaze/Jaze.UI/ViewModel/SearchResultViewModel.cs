using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using System;
using System.Collections.Generic;
using Jaze.UI.Views;
using Prism.Events;
using Prism.Regions;

namespace Jaze.UI.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        #endregion ----- Services -----

        #region ----- Properties -----

        private bool _isSearching = false;

        public bool IsSearching
        {
            get => _isSearching;
            set => SetProperty(ref _isSearching, value);
        }

        private object _listItem = new List<object>();

        public object ListItems
        {
            get => _listItem;
            set => SetProperty(ref _listItem, value);
        }

        private object _selectedItem = null;

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                var parameter = new NavigationParameters
                {
                    {ParamNames.Item, value }
                };
                _regionManager.RequestNavigate(RegionNames.ItemDisplay, nameof(ItemDisplayView), parameter);
            }
        }

        #endregion ----- Properties -----

        #region ----- Constructor -----

        public SearchResultViewModel(IEventAggregator messenger, IRegionManager regionManager)
        {
            _eventAggregator = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _regionManager = regionManager;
            _eventAggregator.GetEvent<PubSubEvent<SearchMessage>>().Subscribe(ProcessSearchMessage);
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