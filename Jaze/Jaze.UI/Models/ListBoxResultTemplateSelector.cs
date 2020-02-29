using System.Windows;
using System.Windows.Controls;

namespace Jaze.UI.Models
{
    internal class ListBoxResultTemplateSelector : DataTemplateSelector
    {
        public DataTemplate JapaneseDataTemplate { get; set; }
        public DataTemplate HanVietDataTemplate { get; set; }
        public DataTemplate KanjiDataTemplate { get; set; }
        public DataTemplate ViJaTemplate { get; set; }
        public DataTemplate GrammarDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
                   DependencyObject container)
        {
            if (item is JaviModel || item is JaenModel)
            {
                return JapaneseDataTemplate;
            }
            if (item is HanVietModel)
            {
                return HanVietDataTemplate;
            }
            if (item is KanjiModel)
            {
                return KanjiDataTemplate;
            }
            if (item is VijaModel)
            {
                return ViJaTemplate;
            }
            if (item is GrammarModel)
            {
                return GrammarDataTemplate;
            }
            return null;
        }
    }
}