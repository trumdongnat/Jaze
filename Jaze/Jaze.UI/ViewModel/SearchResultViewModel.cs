using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using System;
using System.Collections.Generic;
using Prism.Events;

namespace Jaze.UI.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IEventAggregator _messenger;

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
                _messenger.GetEvent<PubSubEvent<ShowItemMessage>>().Publish(new ShowItemMessage(value));
            }
        }

        #endregion ----- Properties -----

        #region ----- Constructor -----

        public SearchResultViewModel(IEventAggregator messenger)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _messenger.GetEvent<PubSubEvent<SearchMessage>>().Subscribe(ProcessSearchMessage);
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