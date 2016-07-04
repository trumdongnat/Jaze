using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jaze.Control
{
    /// <summary>
    /// Interaction logic for ListSearchResult.xaml
    /// </summary>
    public partial class ListSearchResult : UserControl
    {
        public static readonly RoutedEvent ShowEvent;

        static ListSearchResult()
        {
            ListSearchResult.ShowEvent = EventManager.RegisterRoutedEvent(
                "Show", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ListSearchResult));
        }

        public ListSearchResult()
        {
            InitializeComponent();
        }

        public IEnumerable ItemsSource
        {
            get { return listBoxResult.ItemsSource; }
            set { listBoxResult.ItemsSource = value; }
        }

        public bool IsSearching
        {
            get { return searchingIndicator.IsBusy; }
            set { searchingIndicator.IsBusy = value; }
        }
       
        public event RoutedEventHandler Show
        {
            add
            {
                AddHandler(ShowEvent, value);
            }
            remove
            {
                RemoveHandler(ShowEvent, value);
            }
        }
        private void ListBoxResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ListSearchResult.ShowEvent, listBoxResult.SelectedItem));
        }
    }
}
