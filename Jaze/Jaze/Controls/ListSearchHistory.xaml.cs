using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Jaze.Controls
{
    /// <summary>
    /// Interaction logic for ListSearchHistory.xaml
    /// </summary>
    public partial class ListSearchHistory : UserControl
    {
        public ListSearchHistory()
        {
            InitializeComponent();
        }

        public IEnumerable ItemsSource
        {
            get { return listBox.ItemsSource; }
            set { listBox.ItemsSource = value; }
        }
        public event RoutedEventHandler Show
        {
            add
            {
                AddHandler(ListSearchResult.ShowEvent, value);
            }
            remove
            {
                RemoveHandler(ListSearchResult.ShowEvent, value);
            }
        }


        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ListSearchResult.ShowEvent, listBox.SelectedItem));
        }
    }
}
