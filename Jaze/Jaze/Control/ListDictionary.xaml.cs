using System.Windows;
using System.Windows.Controls;
using Jaze.Model;

namespace Jaze.Control
{
    /// <summary>
    /// Interaction logic for ListDictionary.xaml
    /// </summary>
    public partial class ListDictionary : UserControl
    {
        public static readonly RoutedEvent DictionaryChangeEvent;

        static ListDictionary()
        {
            ListDictionary.DictionaryChangeEvent = EventManager.RegisterRoutedEvent(
                "DictionaryChange", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ListDictionary));
        }

        public ListDictionary()
        {
            InitializeComponent();
            InitializeDictionaries();
        }

        public Dictionary CurrentDictionary => ListBoxDics.SelectedItem as Dictionary;

        public event RoutedEventHandler DictionaryChange
        {
            add
            {
                AddHandler(DictionaryChangeEvent, value);
            }
            remove
            {
                RemoveHandler(DictionaryChangeEvent, value);
            }
        }

        private void InitializeDictionaries()
        {
            ListBoxDics.ItemsSource = Dictionary.GetDictionarys();
        }

        private void ListBoxDics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ListDictionary.DictionaryChangeEvent,ListBoxDics.SelectedItem));
        }
    }
}
