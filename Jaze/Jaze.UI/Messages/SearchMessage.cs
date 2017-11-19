using GalaSoft.MvvmLight.Messaging;
using Jaze.UI.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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