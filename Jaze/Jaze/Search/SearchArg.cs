using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.Search
{
    public class SearchArg
    {
        public string SearchKey { get; set; }
        public SearchType Type { get; set; }
    }

    public enum SearchType
    {
        Exact,
        StartWith,
        EndWith,
        Contain
    }
}
