using System.Windows;
using System.Windows.Controls;

namespace Jaze.UI.Views
{
    /// <summary>
    /// Interaction logic for AddGroupView.xaml
    /// </summary>
    public partial class EditGroupView : UserControl
    {
        public EditGroupView()
        {
            InitializeComponent();
        }

        private void SelectAll_OnClick(object sender, RoutedEventArgs e)
        {
            ListView.SelectAll();
        }

        private void UnselectAll_OnClick(object sender, RoutedEventArgs e)
        {
            ListView.UnselectAll();
        }
    }
}