using System.Windows;
using System.Windows.Controls;

namespace Jaze.Views
{
    class ListBoxResultTemplateSelector : DataTemplateSelector
    {
        public DataTemplate JapaneseDataTemplate { get; set; }
        public DataTemplate HanVietDataTemplate { get; set; }
        public DataTemplate KanjiDataTemplate { get; set; }
        public DataTemplate ViJaTemplate { get; set; }
        public DataTemplate GrammarDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
                   DependencyObject container)
        {
            if (item is Model.JaVi || item is Model.JaEn)
            {
                return JapaneseDataTemplate;
            }
            if (item is Model.HanViet)
            {
                return HanVietDataTemplate;
            }
            if (item is Model.Kanji)
            {
                return KanjiDataTemplate;
            }
            if (item is Model.ViJa)
            {
                return ViJaTemplate;
            }
            if (item is Model.Grammar)
            {
                return GrammarDataTemplate;
            }
            return null;
        }
    }
}
