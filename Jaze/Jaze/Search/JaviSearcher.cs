using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.DAO;
using Jaze.Model;

namespace Jaze.Search
{
    static class JaviSearcher
    {
        public static IEnumerable<JaVi> Search(SearchArg searchArg)
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

        private static IEnumerable<JaVi> SearchContain(string key)
        {
            var context = DatabaseContext.Context;
            return context.JaVis.Where(o => o.Word.Contains(key) || o.Kana.Contains(key)).ToArray();
        }

        private static IEnumerable<JaVi> SearchEndWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.JaVis.Where(o => o.Word.EndsWith(key) || o.Kana.EndsWith(key)).ToArray();
        }

        private static IEnumerable<JaVi> SearchStartWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.JaVis.Where(o => o.Word.StartsWith(key) || o.Kana.StartsWith(key)).ToArray();
        }

        private static IEnumerable<JaVi> SearchExact(string key)
        {
            var context = DatabaseContext.Context;
            return context.JaVis.Where(o => o.Word == key || o.Kana == key).ToArray();
        }

        private static IEnumerable<JaVi> GetAll()
        {
            var context = DatabaseContext.Context;
            return context.JaVis.ToArray();
        }
    }
}
