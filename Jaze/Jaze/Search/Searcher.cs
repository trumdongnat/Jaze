using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jaze.Model;

namespace Jaze.Search
{
    public static class Searcher
    {
        public static IEnumerable<object> Search(SearchArg arg)
        {
            if (arg == null)
            {
                return null;
            }
            arg.SearchKey = Regex.Replace(arg.SearchKey, @"\s+", " ");
            arg.SearchKey = arg.SearchKey.Trim();
            switch (arg.Dictionary)
            {
                case DictionaryType.JaVi:
                    return SearchJaVi(arg);
                case DictionaryType.HanViet:
                    return SearchHanViet(arg);
                case DictionaryType.Kanji:
                    return SearchKanji(arg);
                case DictionaryType.ViJa:
                    return SearchViJa(arg);
                case DictionaryType.Grammar:
                    return SearchGrammar(arg);
                case DictionaryType.JaEn:
                    return SearchJaEn(arg);
                default:
                    throw new ArgumentOutOfRangeException(nameof(arg.Dictionary), arg.Dictionary, null);
            }
        }

        public static IEnumerable<HanViet> SearchHanViet(SearchArg searchArg)
        {
            return HanVietSearcher.Search(searchArg);
        }

        public static IEnumerable<Kanji> SearchKanji(SearchArg searchArg)
        {
            return KanjiSearcher.Search(searchArg);
        }

        public static IEnumerable<ViJa> SearchViJa(SearchArg searchArg)
        {
            return VijaSearcher.Search(searchArg);
        }

        public static IEnumerable<Grammar> SearchGrammar(SearchArg searchArg)
        {
            return GrammarSearcher.Search(searchArg);
        }

        public static IEnumerable<JaEn> SearchJaEn(SearchArg searchArg)
        {
            return JaenSearcher.Search(searchArg);
        }

        public static IEnumerable<JaVi> SearchJaVi(SearchArg arg)
        {
            return JaviSearcher.Search(arg);
        }
    }

   
}