using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Jaze.UI.ViewModel
{
    internal class SearchBarViewModel : ViewModelBase
    {
        private RelayCommand _myCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand MyCommand
        {
            get
            {
                return _myCommand
                    ?? (_myCommand = new RelayCommand(
                    () =>
                    {
                    },
                    () => true));
            }
        }
    }
}