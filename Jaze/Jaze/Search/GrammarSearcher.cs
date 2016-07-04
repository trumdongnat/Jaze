using System;
using System.Collections.Generic;
using System.Linq;
using Jaze.DAO;
using Jaze.Model;

namespace Jaze.Search
{
    static class GrammarSearcher
    {
        public static IEnumerable<Grammar> Search(SearchArg searchArg)
        {
            var key = searchArg.SearchKey;

            if (string.IsNullOrWhiteSpace(key))
            {
                return GetAll();
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

        private static IEnumerable<Grammar> SearchContain(string key)
        {
            var context = DatabaseContext.Context;
            return context.Grammars.Where(o => o.Struct.Contains(key) || o.Meaning.Contains(key)).ToArray();
        }

        private static IEnumerable<Grammar> SearchEndWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.Grammars.Where(o => o.Struct.EndsWith(key) || o.Meaning.EndsWith(key)).ToArray();
        }

        private static IEnumerable<Grammar> SearchStartWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.Grammars.Where(o => o.Struct.StartsWith(key) || o.Meaning.StartsWith(key)).ToArray();
        }

        private static IEnumerable<Grammar> SearchExact(string key)
        {
            var context = DatabaseContext.Context;
            return context.Grammars.Where(o => o.Struct == key || o.Meaning == key).ToArray();
        }

        private static IEnumerable<Grammar> GetAll()
        {
            var context = DatabaseContext.Context;
            return context.Grammars.ToArray();
        }
    }
}
