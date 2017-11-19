using Jaze.UI.Definitions;

namespace Jaze.UI.Messages
{
    public class SearchMessage
    {
        public SearchStates SearchState { get; set; }

        public DictionaryType DictionaryType { get; set; }

        public object Result { get; set; }

        public SearchMessage(SearchStates searchState, DictionaryType dictionaryType, object result)
        {
            SearchState = searchState;
            DictionaryType = dictionaryType;
            Result = result;
        }
    }
}