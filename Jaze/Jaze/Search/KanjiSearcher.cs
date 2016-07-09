using System;
using System.Collections.Generic;
using System.Linq;
using Jaze.DAO;
using Jaze.Model;
using Jaze.Util;

namespace Jaze.Search
{
    static class KanjiSearcher
    {
        public static IEnumerable<Kanji> Search(SearchArg searchArg)
        {
            var key = searchArg.SearchKey;
            //
            if (string.IsNullOrWhiteSpace(key))
            {
                return GetAll();
            }
            //if search key contain multi kanji
            var arr = ConvertStringUtil.FilterCharsInString(key, CharSet.Kanji);
            if (arr.Count>0)
            {
                return LoadKanji(arr);
            }
            //if search key is vietnamese sentence
            if (key.Contains(" "))
            {
                return SearchVietNameseSentence(key);
            }
            //other case
            switch (searchArg.Option)
            {
                case SearchOption.Exact:
                    return SearchExact(key);
                case SearchOption.StartWith:
                    return SearchStartWith(key);
                case SearchOption.EndWith:
                    return SearchEndWith(key);
                case SearchOption.Contain:
                    return SearchContain(key);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IEnumerable<Kanji> SearchVietNameseSentence(string key)
        {
            var arr = key.Split(' ');
            var context = DatabaseContext.Context;
            var list = new List<Kanji>();
            foreach (var s in arr)
            {
                var kanjis = context.Kanjis.Where(k => k.HanViet == s);
                foreach (var kanji in kanjis)
                {
                    if (!list.Contains(kanji))
                    {
                        list.Add(kanji);
                    }
                }
            }
            return list;
        }

        private static IEnumerable<Kanji> LoadKanji(IList<char> arr)
        {
            var context = DatabaseContext.Context;
            return arr.Select(c => "" + c).Select(s => context.Kanjis.FirstOrDefault(kanji => kanji.Word == s)).ToList();
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
