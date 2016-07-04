using System;
using System.Collections.Generic;
using System.Linq;
using Jaze.DAO;
using Jaze.Model;

namespace Jaze.Search
{
    internal class HanVietSearcher
    {
        public static IEnumerable<HanViet> Search(SearchArg searchArg)
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

        private static IEnumerable<HanViet> SearchContain(string key)
        {
            var context = DatabaseContext.Context;
            return context.HanViets.Where(hv => hv.Word.Contains(key) || hv.Reading.Contains(key)).ToArray();
        }

        private static IEnumerable<HanViet> SearchEndWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.HanViets.Where(hv => hv.Word.EndsWith(key) || hv.Reading.EndsWith(key)).ToArray();
        }

        private static IEnumerable<HanViet> SearchStartWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.HanViets.Where(hv => hv.Word.StartsWith(key) || hv.Reading.StartsWith(key)).ToArray();
        }

        private static IEnumerable<HanViet> SearchExact(string key)
        {
            var context = DatabaseContext.Context;
            return context.HanViets.Where(hv => hv.Word == key || hv.Reading == key).ToArray();
        }

        private static IEnumerable<HanViet> GetAll()
        {
            var context = DatabaseContext.Context;
            return context.HanViets.ToArray();
        }
    }
}
