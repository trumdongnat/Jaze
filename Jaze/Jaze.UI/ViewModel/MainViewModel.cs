using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Jaze.UI.Messages;

namespace Jaze.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region ----- Services -----

        private IMessenger _messenger;

        #endregion ----- Services -----

        #region ----- Is Show Quick View -----

        /// <summary>
        /// The <see cref="IsShowQuickView" /> property's name.
        /// </summary>
        public const string IsShowQuickViewPropertyName = "IsShowQuickView";

        private bool _isShowQuickView = false;

        /// <summary>
        /// Sets and gets the IsShowQuickView property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsShowQuickView
        {
            get
            {
                return _isShowQuickView;
            }
            set
            {
                Set(IsShowQuickViewPropertyName, ref _isShowQuickView, value);
            }
        }

        #endregion ----- Is Show Quick View -----

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Register<QuickViewMessage>(this, message =>
            {
                IsShowQuickView = true;
            });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}