using Jaze.UI.Definitions;

namespace Jaze.UI.Messages
{
    public class QuickViewMessage
    {
        public DictionaryType DictionaryType { get; set; }
        public string Word { get; set; }

        public QuickViewMessage(DictionaryType dictionaryType, string word)
        {
            DictionaryType = dictionaryType;
            Word = word;
        }
    }
}