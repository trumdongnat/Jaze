﻿using Jaze.UI.Messages;
using Jaze.UI.Views;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Media;
using Jaze.UI.Notification;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;

namespace Jaze.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
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

        public MainViewModel(IEventAggregator eventAggregator, IDialogCoordinator dialogCoordinator)
        {
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
        }

        #region Interactions

        public InteractionRequest<IShowKanjiPartNotification> ShowKanjiPartRequest { get; set; }

        #endregion Interactions
    }
}