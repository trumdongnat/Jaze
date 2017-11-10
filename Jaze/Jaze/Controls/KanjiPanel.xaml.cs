using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Jaze.Documents;
using Jaze.Domain.Entities;

namespace Jaze.Controls
{
    /// <summary>
    /// Interaction logic for KanjiPanel.xaml
    /// </summary>
    public partial class KanjiPanel : UserControl
    {
        public KanjiPanel()
        {
            InitializeComponent();
        }

        private void ButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListKanji.Items != null)
            {
                Clipboard.SetText(string.Concat(ListKanji.Items.Cast<Kanji>().Select(k=>k.Word)));
            }
        }

        private void ListKanji_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListKanji.SelectedItem != null)
            {
                var kanji = ListKanji.SelectedItem as Kanji;
                DocumentViewer.Document = Builder.Build(kanji);
            }
        }
    }
}
