using System;
using System.Collections.Generic;
using Jaze.Model;

namespace Jaze.Search
{
    public static class Searcher
    {
        public static IEnumerable<object> Search(DictionaryType dictionary, SearchArg arg)
        {
            if (arg == null)
            {
                return null;
            }
            arg.SearchKey = arg.SearchKey.Trim();
            switch (dictionary)
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
                    throw new ArgumentOutOfRangeException(nameof(dictionary), dictionary, null);
            }
        }

        private static IEnumerable<HanViet> SearchHanViet(SearchArg searchArg)
        {
            return HanVietSearcher.Search(searchArg);
        }

        private static IEnumerable<Kanji> SearchKanji(SearchArg searchArg)
        {
            return KanjiSearcher.Search(searchArg);
        }

        private static IEnumerable<ViJa> SearchViJa(SearchArg searchArg)
        {
            return VijaSearcher.Search(searchArg);
        }

        private static IEnumerable<Grammar> SearchGrammar(SearchArg searchArg)
        {
            return GrammarSearcher.Search(searchArg);
        }

        private static IEnumerable<JaEn> SearchJaEn(SearchArg searchArg)
        {
            return JaenSearcher.Search(searchArg);
        }

        private static IEnumerable<JaVi> SearchJaVi(SearchArg arg)
        {
            return JaviSearcher.Search(arg);
        }
    }

   
}