using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain;
using Jaze.Domain.Entity;
using Jaze.Model;
using Jaze.Util;

namespace Jaze.Search
{
    static class JaenSearcher
    {
        public static IEnumerable<JaEn> Search(SearchArgs searchArgs)
        {
            var key = searchArgs.SearchKey;

            if (string.IsNullOrWhiteSpace(key))
            {
                //return GetAll();
                return null;
            }

            key = key.Contains("-") ? StringUtil.ConvertRomaji2Katakana(key) : StringUtil.ConvertRomaji2Hiragana(key);

            switch (searchArgs.Option)
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

        private static IEnumerable<JaEn> SearchContain(string key)
        {
            var context = JazeDatabaseContext.Context;
            return context.JaEns.Where(o => o.Word.Contains(key) || o.Kana.Contains(key)).ToArray();
        }

        private static IEnumerable<JaEn> SearchEndWith(string key)
        {
            var context = JazeDatabaseContext.Context;
            return context.JaEns.Where(o => o.Word.EndsWith(key) || o.Kana.EndsWith(key)).ToArray();
        }

        private static IEnumerable<JaEn> SearchStartWith(string key)
        {
            var context = JazeDatabaseContext.Context;
            return context.JaEns.Where(o => o.Word.StartsWith(key) || o.Kana.StartsWith(key)).ToArray();
        }

        private static IEnumerable<JaEn> SearchExact(string key)
        {
            var context = JazeDatabaseContext.Context;
            //at stat
            var keyStart = key + " ";
            //at middle
            var keyMiddle = " " + key + " ";
            //at end
            var keyEnd = " " + key;
            return context.JaEns.Where(o => o.Word == key || o.Kana == key || o.Kana.StartsWith(keyStart) || o.Kana.Contains(keyMiddle) || o.Kana.EndsWith(keyEnd)).ToArray();
        }

        private static IEnumerable<JaEn> GetAll()
        {
            var context = JazeDatabaseContext.Context;
            return context.JaEns.ToArray();
        }
    }
}
