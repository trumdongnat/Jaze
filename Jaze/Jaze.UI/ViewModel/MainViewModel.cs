using Jaze.UI.Messages;
using Jaze.UI.Views;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Media;
using Prism.Events;

namespace Jaze.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IEventAggregator _eventAggregator;

        #endregion ----- Services -----

        #region ----- Is Show Quick View -----

        private bool _isShowQuickView = false;

        public bool IsShowQuickView
        {
            get => _isShowQuickView;
            set => SetProperty(ref _isShowQuickView, value);
        }

        #endregion ----- Is Show Quick View -----

        public MainViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PubSubEvent<QuickViewMessage>>().Subscribe(message =>
           {
               IsShowQuickView = true;
           });
            _eventAggregator.GetEvent<PubSubEvent<ShowPartsMessage>>().Subscribe(message =>
            {
                var window = new MetroWindow();
                window.Title = "Kanji Part";

                window.Content = kanjiPartView;
                window.Width = 800;
                window.Height = 600;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.BorderThickness = new Thickness(1.0);
                window.BorderBrush = Brushes.Black;

                window.ShowDialog();
            });
        }

        private readonly KanjiPart kanjiPartView = new KanjiPart();
    }
}