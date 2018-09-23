using System.Diagnostics;
using Jaze.UI.Messages;
using Jaze.UI.Views;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Media;
using Jaze.UI.Definitions;
using Jaze.UI.Notification;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Logging;
using Prism.Regions;

namespace Jaze.UI.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogCoordinator _dialogCoordinator;

        #endregion ----- Services -----

        #region ----- Is Show Quick View -----

        private bool _isShowQuickView = false;

        public bool IsShowQuickView
        {
            get => _isShowQuickView;
            set => SetProperty(ref _isShowQuickView, value);
        }

        #endregion ----- Is Show Quick View -----

        public ShellViewModel(IEventAggregator eventAggregator, IDialogCoordinator dialogCoordinator, IRegionManager regionManager, IUnityContainer unityContainer, ILoggerFacade logger)
        {
            Debug.WriteLine("ShellViewModel");
            _eventAggregator = eventAggregator;
            _dialogCoordinator = dialogCoordinator;
            _eventAggregator.GetEvent<PubSubEvent<QuickViewMessage>>().Subscribe(message =>
           {
               IsShowQuickView = true;
           });
            ShowKanjiPartRequest = new InteractionRequest<IShowKanjiPartNotification>();
            _eventAggregator.GetEvent<PubSubEvent<ShowPartsMessage>>().Subscribe(message =>
            {
                ShowKanjiPartRequest.Raise(new ShowKanjiPartNofitication() { Parts = message.Parts, Title = "Kanji Part" });
            });
            regionManager.RegisterViewWithRegion(RegionNames.SearchPanel, typeof(SearchPanel));
            regionManager.RegisterViewWithRegion(RegionNames.WordGroup, typeof(WordGroupPanel));
        }

        #region Interactions

        public InteractionRequest<IShowKanjiPartNotification> ShowKanjiPartRequest { get; set; }

        #endregion Interactions
    }
}