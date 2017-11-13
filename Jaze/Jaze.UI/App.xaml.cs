using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace Jaze.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
