using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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