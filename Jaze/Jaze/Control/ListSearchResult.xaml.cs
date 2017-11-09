using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Jaze.Domain.Entity;
using Jaze.Model;

namespace Jaze.Control
{
    /// <summary>
    ///     Interaction logic for ListSearchResult.xaml
    /// </summary>
    public partial class ListSearchResult : UserControl
    {
        public static readonly RoutedEvent ShowEvent;

        private readonly List<RadioButton> _kanjiSortButtons,
            _hanVietSortButtons,
            _javiSortButtons,
            _jaenSortButtons,
            _vijaSortButtons,
            _grammarSortButtons;

        static ListSearchResult()
        {
            ShowEvent = EventManager.RegisterRoutedEvent(
                "Show", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (ListSearchResult));
        }

        public ListSearchResult()
        {
            InitializeComponent();
            _kanjiSortButtons = new List<RadioButton>
            {
                CreateRadioButton("Kanji", "Word", ListSortDirection.Ascending),
                CreateRadioButton("Hán Việt", "HanViet", ListSortDirection.Ascending),
                CreateRadioButton("Số nét", "Stroke", ListSortDirection.Ascending),
                CreateRadioButton("Level", "Level.Name", ListSortDirection.Descending),
                CreateRadioButton("Phổ biến", "Frequence", ListSortDirection.Ascending),
                CreateRadioButton("Bộ thủ", "Radical.Id", ListSortDirection.Ascending)
            };

            _hanVietSortButtons = new List<RadioButton>
            {
                CreateRadioButton("Hán tự", "Word", ListSortDirection.Ascending),
                CreateRadioButton("Hán Việt", "Reading", ListSortDirection.Ascending),
                CreateRadioButton("Số từ", "Word.Length", ListSortDirection.Ascending)
            };

            _javiSortButtons = new List<RadioButton>
            {
                CreateRadioButton("Từ vựng", "Word", ListSortDirection.Ascending),
                CreateRadioButton("Kana", "Kana", ListSortDirection.Ascending),
                CreateRadioButton("Độ dài từ", "Word.Length", ListSortDirection.Ascending)
            };

            _jaenSortButtons = _javiSortButtons;

            _vijaSortButtons = new List<RadioButton>
            {
                CreateRadioButton("Từ vựng", "Word", ListSortDirection.Ascending)
            };

            _grammarSortButtons = new List<RadioButton>
            {
                CreateRadioButton("Level", "Level.Name", ListSortDirection.Ascending)
            };
        }

        public IEnumerable ItemsSource
        {
            get { return listBoxResult.ItemsSource; }
            set
            {
                listBoxResult.ItemsSource = value;
                ChangeSortPanel();
            }
        }

        public bool IsSearching
        {
            get { return searchingIndicator.IsBusy; }
            set { searchingIndicator.IsBusy = value; }
        }

        private RadioButton CreateRadioButton(string content, string sortProperty,
            ListSortDirection sortDirection)
        {
            var button = new RadioButton
            {
                Content = content,
                Tag = new SortDescription(sortProperty, sortDirection),
                GroupName = "sort"
            };
            button.Checked += SortButtonOnChecked;
            return button;
        }

        private void SortButtonOnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            listBoxResult.Items.SortDescriptions.Clear();
            var button = sender as RadioButton;
            if (button != null)
            {
                listBoxResult.Items.SortDescriptions.Add((SortDescription) button.Tag);
            }
        }

        private void ChangeSortPanel()
        {
            sortPanel.Children.Clear();
            var list = new List<RadioButton>();
            if (listBoxResult.ItemsSource is IEnumerable<Kanji>)
            {
                list = _kanjiSortButtons;
            }
            else if (listBoxResult.ItemsSource is IEnumerable<HanViet>)
            {
                list = _hanVietSortButtons;
            }
            else if (listBoxResult.ItemsSource is IEnumerable<ViJa>)
            {
                list = _vijaSortButtons;
            }
            else if (listBoxResult.ItemsSource is IEnumerable<JaVi>)
            {
                list = _javiSortButtons;
            }
            else if (listBoxResult.ItemsSource is IEnumerable<JaEn>)
            {
                list = _jaenSortButtons;
            }
            else if (listBoxResult.ItemsSource is IEnumerable<Grammar>)
            {
                list = _grammarSortButtons;
            }

            foreach (var radioButton in list)
            {
                sortPanel.Children.Add(radioButton);
            }
        }

        public event RoutedEventHandler Show
        {
            add { AddHandler(ShowEvent, value); }
            remove { RemoveHandler(ShowEvent, value); }
        }

        private void ListBoxResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ShowEvent, listBoxResult.SelectedItem));
        }
    }
}