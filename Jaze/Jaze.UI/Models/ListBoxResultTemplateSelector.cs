using System.Windows;
using System.Windows.Controls;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
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
            if (item is JaVi || item is JaEn)
            {
                return JapaneseDataTemplate;
            }
            if (item is HanViet)
            {
                return HanVietDataTemplate;
            }
            if (item is Kanji)
            {
                return KanjiDataTemplate;
            }
            if (item is ViJa)
            {
                return ViJaTemplate;
            }
            if (item is Grammar)
            {
                return GrammarDataTemplate;
            }
            return null;
        }
    }
}
