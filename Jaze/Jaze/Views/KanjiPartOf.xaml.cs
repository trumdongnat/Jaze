using System.Linq;
using System.Windows;
using Jaze.DAO;
using Jaze.Model;

namespace Jaze.Views
{
    /// <summary>
    ///     Interaction logic for KanjiPartOf.xaml
    /// </summary>
    public partial class KanjiPartOf : Window
    {
        public KanjiPartOf(Kanji kanji)
        {
            InitializeComponent();
            //set title
            Title = kanji.Word + " is part of:";
            //get part of
            var arr =
                DatabaseContext.Context.Kanjis.Where(k => k.Component.Contains(kanji.Word))
                    .Select(k => k.Word)
                    .ToArray();
            textBox.Text = string.Concat(arr);
        }

        private void ButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBox.Text);
        }
    }
}