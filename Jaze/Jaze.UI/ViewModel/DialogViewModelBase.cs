using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.ViewModel
{
    public abstract class DialogViewModelBase : ViewModelBase, IDialogAware
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public abstract event Action<IDialogResult> RequestClose;

        public virtual bool CanCloseDialog() {
            return true;
        }

        public virtual void OnDialogClosed() { }

        public abstract void OnDialogOpened(IDialogParameters parameters);
    }
}
