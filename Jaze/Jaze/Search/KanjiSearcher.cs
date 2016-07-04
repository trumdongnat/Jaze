using System;
using System.Collections.Generic;
using System.Linq;
using Jaze.DAO;
using Jaze.Model;

namespace Jaze.Search
{
    static class KanjiSearcher
    {
        public static IEnumerable<Kanji> Search(SearchArg searchArg)
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

        private static IEnumerable<Kanji> SearchContain(string key)
        {
            var context = DatabaseContext.Context;
            return context.Kanjis.Where(kanji => kanji.Word.Contains(key) || kanji.HanViet.Contains(key)).ToArray();
        }

        private static IEnumerable<Kanji> SearchEndWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.Kanjis.Where(kanji => kanji.Word.EndsWith(key) || kanji.HanViet.EndsWith(key)).ToArray();
        }

        private static IEnumerable<Kanji> SearchStartWith(string key)
        {
            var context = DatabaseContext.Context;
            return context.Kanjis.Where(kanji => kanji.Word.StartsWith(key) || kanji.HanViet.StartsWith(key)).ToArray();
        }

        private static IEnumerable<Kanji> SearchExact(string key)
        {
            var context = DatabaseContext.Context;
            return context.Kanjis.Where(kanji => kanji.Word == key || kanji.HanViet == key).ToArray();
        }

        private static IEnumerable<Kanji> GetAll()
        {
            var context = DatabaseContext.Context;
            return context.Kanjis.ToArray();
        }
    }
}
