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

        private void FlowDocumentScrollViewer_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(sender is FlowDocumentScrollViewer viewer)
            {
                MenuPopup.IsOpen = !string.IsNullOrWhiteSpace(viewer.Selection?.Text);
            }
        }

        private void FlowDocumentScrollViewer_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MenuPopup.IsOpen = false;
        }

        private void QuickViewButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if(DataContext is ItemDisplayViewModel viewModel)
            {
                var text = DocumentViewer.Selection?.Text;
                if (viewModel.QuickViewCommand.CanExecute(text))
                {
                    viewModel.QuickViewCommand.Execute(text);
                }
            }
        }

        private void CopyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ItemDisplayViewModel viewModel)
            {
                var text = DocumentViewer.Selection?.Text;
                if (viewModel.CopyTextCommand.CanExecute(text))
                {
                    viewModel.CopyTextCommand.Execute(text);
                }
            }

        }
    }
}