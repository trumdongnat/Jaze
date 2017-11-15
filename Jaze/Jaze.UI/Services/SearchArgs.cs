using Jaze.UI.Definitions;
using Jaze.UI.Models;

namespace Jaze.UI.Services
{
    public class SearchArgs
    {
        public SearchArgs(string searchKey, SearchOption option)
        {
            SearchKey = searchKey;
            Option = option;
        }

        public SearchArgs(string searchKey, SearchOption option, DictionaryType dictionary)
        {
            SearchKey = searchKey;
            Option = option;
            Dictionary = dictionary;
        }

        public string SearchKey { get; set; }
        public SearchOption Option { get; set; }
        public DictionaryType Dictionary { get; set; }
    }
}