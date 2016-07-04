using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.DAO;
using Jaze.Model;

namespace Jaze.Search
{
    static class VijaSearcher
    {
        public static IEnumerable<ViJa> Search(SearchArg searchArg)
        {
            var key = searchArg.SearchKey;

            if (string.IsNullOrWhiteSpace(key))
            {
                //return GetAll();
                return null;
            }

            switch (searchArg.Type)
            {
                case SearchType.Exact:
                    return SearchExact(key);
                case SearchType.StartWith:
                    return SearchStartWith(key);
                case SearchType.EndWith:
                    return SearchEndWith(key);
                case SearchType.Contain:
                    return SearchContain(key);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IEnumerable<ViJa> SearchContain(string key)
        {
            var context = DatabaseContext.Context;
            return context.ViJas.Where(o => o.Word.Contains(key)).ToArray();
        }

        private static IEnumerable<ViJa> SearchEndWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.ViJas.Where(o => o.Word.EndsWith(key)).ToArray();
        }

        private static IEnumerable<ViJa> SearchStartWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.ViJas.Where(o => o.Word.StartsWith(key)).ToArray();
        }

        private static IEnumerable<ViJa> SearchExact(string key)
        {
            var context = DatabaseContext.Context;
            return context.ViJas.Where(o => o.Word == key).ToArray();
        }

        private static IEnumerable<ViJa> GetAll()
        {
            var context = DatabaseContext.Context;
            return context.ViJas.ToArray();
        }
    }
}
