using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Jaze.UI.ViewModel
{
    class SearchBarViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="MyProperty" /> property's name.
        /// </summary>
        public const string MyPropertyPropertyName = "MyProperty";

        private bool _myProperty = false;

        /// <summary>
        /// Sets and gets the MyProperty property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public bool MyProperty
        {
            get
            {
                return _myProperty;
            }
            set
            {
                Set(MyPropertyPropertyName, ref _myProperty, value, true);
            }
        }
    }
}
