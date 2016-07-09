using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Jaze.Model;

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
            set
            {
                listBoxResult.ItemsSource = value;
                ChangeSortDesciption();
            }
        }

        private void ChangeSortDesciption()
        {
            listBoxResult.Items.SortDescriptions.Clear();
            if (listBoxResult.ItemsSource is IEnumerable<Grammar>)
            {
                listBoxResult.Items.SortDescriptions.Add(new SortDescription("Level.Name", ListSortDirection.Ascending));
            }
            //TODO config sort desciption
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
