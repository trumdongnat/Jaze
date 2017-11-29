using System.Windows.Controls;
using System.Windows.Documents;
using Jaze.UI.ViewModel;

namespace Jaze.UI.Views
{
    /// <summary>
    /// Interaction logic for DisplayView.xaml
    /// </summary>
    public partial class ItemDisplayView : UserControl
    {
        public ItemDisplayView()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Hyperlink hyperlink && DataContext is ItemDisplayViewModel viewmodel)
            {
                if (viewmodel.HyperlinkClickCommand.CanExecute(hyperlink))
                {
                    viewmodel.HyperlinkClickCommand.Execute(hyperlink);
                }
            }
        }
    }
}