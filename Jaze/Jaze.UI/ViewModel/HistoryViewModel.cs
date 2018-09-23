using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Entities;
using Jaze.UI.Definitions;
using Jaze.UI.Models;
using Jaze.UI.Repository;
using Jaze.UI.Views;
using Prism.Commands;
using Prism.Regions;

namespace Jaze.UI.ViewModel
{
    public class HistoryViewModel : ViewModelBase
    {
        private readonly IUserDataRepository _userDataRepository;
        private IRegionManager _regionManager;

        #region Properties

        public IEnumerable<HistoryTimeRange> HistoryTimeRanges =>
            Enum.GetValues(typeof(HistoryTimeRange)).Cast<HistoryTimeRange>();

        private HistoryTimeRange _historyTimeRange;

        public HistoryTimeRange HistoryTimeRange
        {
            get => _historyTimeRange;
            set => SetProperty(ref _historyTimeRange, value, OnHistoryTimeRangeChanged);
        }

        private void OnHistoryTimeRangeChanged()
        {
            ExecuteRefreshCommand();
        }

        private ObservableCollection<HistoryModel> _historyCollection = new ObservableCollection<HistoryModel>();

        public ObservableCollection<HistoryModel> HistoryCollection
        {
            get => _historyCollection;
            set => SetProperty(ref _historyCollection, value);
        }

        #endregion Properties

        #region Commands

        private DelegateCommand _refreshCommand;

        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(ExecuteRefreshCommand));

        private void ExecuteRefreshCommand()
        {
            Init();
        }

        private DelegateCommand<object> _showItemCommand;

        public DelegateCommand<object> ShowItemCommand =>
            _showItemCommand ?? (_showItemCommand = new DelegateCommand<object>(ExecuteShowItemCommand));

        private void ExecuteShowItemCommand(object item)
        {
            var parameter = new NavigationParameters
            {
                {ParamNames.Item, item }
            };
            _regionManager.RequestNavigate(RegionNames.HistoryItemDisplay, nameof(ItemDisplayView), parameter);
        }

        #endregion Commands

        public HistoryViewModel(IUserDataRepository userDataRepository, IRegionManager regionManager)
        {
            _userDataRepository = userDataRepository;
            _regionManager = regionManager;
            Init();
        }

        private async void Init()
        {
            HistoryCollection.Clear();
            var now = DateTime.Now;
            DateTime from;
            switch (HistoryTimeRange)
            {
                case HistoryTimeRange.LastHour:
                    from = now.AddHours(-1);
                    break;

                case HistoryTimeRange.Last24Hours:
                    from = now.AddHours(-24);
                    break;

                case HistoryTimeRange.Last7Days:
                    from = now.AddDays(-7);
                    break;

                case HistoryTimeRange.Last4Weeks:
                    from = now.AddDays(-4 * 7);
                    break;

                case HistoryTimeRange.AllTime:
                    from = DateTime.MinValue;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var histories = from == DateTime.MinValue ? await _userDataRepository.GetListHistory()
                : await _userDataRepository.GetListHistory(from);
            foreach (var history in histories)
            {
                await _userDataRepository.LoadFull(history);
            }
            HistoryCollection.AddRange(histories.OrderByDescending(history => history.LastTime));
        }
    }
}