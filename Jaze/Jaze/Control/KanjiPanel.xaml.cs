using System;
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
using Jaze.Document;
using Jaze.Domain.Entity;
using Jaze.Model;

namespace Jaze.Control
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
