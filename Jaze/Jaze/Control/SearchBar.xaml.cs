using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Jaze.Search;
using SearchOption = Jaze.Search.SearchOption;

namespace Jaze.Control
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar
    {
        public static readonly RoutedEvent SearchEvent;
        public static readonly RoutedEvent SearchOptionChangeEvent;
        static SearchBar()
        {
            SearchBar.SearchEvent = EventManager.RegisterRoutedEvent(
                "Search", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (SearchBar));
            SearchBar.SearchOptionChangeEvent = EventManager.RegisterRoutedEvent(
                "SearchOptionChange", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SearchBar));
        }


        public SearchBar()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler Search
        {
            add
            {
                AddHandler(SearchEvent,value);
            }
            remove
            {
                RemoveHandler(SearchEvent,value);
            }
        }

        public event RoutedEventHandler SearchOptionChange
        {
            add
            {
                AddHandler(SearchOptionChangeEvent, value);
            }
            remove
            {
                RemoveHandler(SearchOptionChangeEvent, value);
            }
        }
        public string Text
        {
            get { return TextBoxSearch.Text; }
            set { TextBoxSearch.Text = value; }
        }

        public SearchArgs SearchArgs => new SearchArgs(TextBoxSearch.Text, SearchOption);

        public SearchOption SearchOption
        {
            get
            {
                if (RadioButtonExact.IsChecked != null && (bool)RadioButtonExact.IsChecked)
                {
                    return SearchOption.Exact;
                }
                if (RadioButtonStart.IsChecked != null && (bool)RadioButtonStart.IsChecked)
                {
                    return  SearchOption.StartWith;
                }
                if (RadioButtonEnd.IsChecked != null && (bool)RadioButtonEnd.IsChecked)
                {
                    return  SearchOption.EndWith;
                }
                if (RadioButtonMiddle.IsChecked != null && (bool)RadioButtonMiddle.IsChecked)
                {
                    return SearchOption.Contain;
                }
                return SearchOption.Exact;
            }
            set
            {
                switch (value)
                {
                    case SearchOption.Exact:
                        RadioButtonExact.IsChecked = true;
                        break;
                    case SearchOption.StartWith:
                        RadioButtonStart.IsChecked = true;
                        break;
                    case SearchOption.EndWith:
                        RadioButtonEnd.IsChecked = true;
                        break;
                    case SearchOption.Contain:
                        RadioButtonMiddle.IsChecked = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void RaiseSearchEvent()
        {
            RaiseEvent(new RoutedEventArgs(SearchEvent, SearchArgs));
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            RaiseSearchEvent();
        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RaiseSearchEvent();
            }
        }

        private void RadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            RaiseEvent(new RoutedEventArgs(SearchOptionChangeEvent, SearchOption));
        }

        private void ButtonPasteSearch_Click(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = Clipboard.GetText();
            RaiseSearchEvent();
        }
    }
}