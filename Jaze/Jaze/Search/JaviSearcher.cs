using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jaze.DAO;
using Jaze.Model;
using Jaze.Util;

namespace Jaze.Search
{
    static class JaviSearcher
    {
        private const string HANVIET_CACHE_FILE = "jv_hv";
        private static List<JaviHanVietCache> _javiHanVietCaches;
        static JaviSearcher()
        {
            _javiHanVietCaches = new List<JaviHanVietCache>();
            string cacheFolder = System.IO.Path.Combine(App.BaseDictionary, "Caches");
            var cachePath = System.IO.Path.Combine(cacheFolder, HANVIET_CACHE_FILE);
            System.IO.Directory.CreateDirectory(cacheFolder);
            if (System.IO.File.Exists(cachePath))
            {
                //read from created cache file
                foreach (var line in System.IO.File.ReadAllLines(cachePath))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        try
                        {
                            var arr = line.Split('\t');
                            var simpleJavi = new JaviHanVietCache();
                            simpleJavi.DbId = int.Parse(arr[0]);
                            simpleJavi.HanViet = new List<string>();
                            for (int i = 1; i < arr.Length; i++)
                            {
                                simpleJavi.HanViet.Add(arr[i]);
                            }
                            _javiHanVietCaches.Add(simpleJavi);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }
            else
            {
                //read from database
                var context = DatabaseContext.Context;
                var hvDic = context.Kanjis.ToDictionary(kanji => kanji.Word, kanji => kanji.HanViet);
                var javis = context.JaVis.Select(javi => new {javi.Id, javi.Word}).ToList();
                foreach (var javi in javis)
                {
                    var simpleJavi = new JaviHanVietCache();
                    simpleJavi.DbId = javi.Id;
                    simpleJavi.HanViet = new List<string>();
                    foreach (var c in javi.Word.ToCharArray())
                    {
                        var key = c.ToString();
                        if (hvDic.ContainsKey(key))
                        {
                            simpleJavi.HanViet.Add(hvDic[key]);
                        }
                    }
                    _javiHanVietCaches.Add(simpleJavi);
                }
                //save to cache file
                var writer = System.IO.File.AppendText(cachePath);
                foreach (var simpleJavi in _javiHanVietCaches)
                {
                    if (simpleJavi.HanViet.Count > 0)
                    {
                        writer.Write("{0}\t", simpleJavi.DbId);
                        var hvCount = simpleJavi.HanViet.Count;
                        for (int i = 0; i < hvCount - 1; i++)
                        {
                            writer.Write("{0}\t", simpleJavi.HanViet[i]);
                        }
                        writer.WriteLine("{0}", simpleJavi.HanViet[hvCount - 1]);
                    }
                }
                writer.Close();
            }
            foreach (var simpleJavi in _javiHanVietCaches)
            {
                simpleJavi.HanViet2 = new List<List<string>>();
                foreach (var s in simpleJavi.HanViet)
                {
                    simpleJavi.HanViet2.Add(s.Split(',').ToList());
                }
            }
        }
        public static IEnumerable<JaVi> Search(SearchArgs searchArgs)
        {
            var rawKey = searchArgs.SearchKey;

            if (string.IsNullOrWhiteSpace(rawKey))
            {
                //return GetAll();
                return null;
            }
            var key = rawKey.Contains("-") ? StringUtil.ConvertRomaji2Katakana(rawKey) : StringUtil.ConvertRomaji2Hiragana(rawKey);
            IEnumerable<JaVi> resultJv = new JaVi[] {};
            IEnumerable<JaVi> resultHv = new JaVi[] { };
            if (StringUtil.IsJapanese(key))
            {
                resultJv = SearchJapanese(new SearchArgs(key, searchArgs.Option));
            }

            if (rawKey.Split(' ').All(StringUtil.IsVietnamese))
            {
                resultHv = SearchHanViet(searchArgs);
            }
            return resultJv.Union(resultHv);
        }

        private static IEnumerable<JaVi> SearchJapanese(SearchArgs searchArgs)
        {
            var key = searchArgs.SearchKey;
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

        private static IEnumerable<JaVi> SearchHanViet(SearchArgs searchArgs)
        {
            var key = searchArgs.SearchKey;
            var arr = key.Split(' ');
            var kanjiOfWord = arr.Length;
            var result = new List<JaVi>();
            var context = DatabaseContext.Context;
            foreach (var simpleJavi in _javiHanVietCaches)
            {
                if (simpleJavi.HanViet.Count == kanjiOfWord)
                {
                    bool flag = true;
                    for (int i = 0; i < kanjiOfWord; i++)
                    {
                        if (simpleJavi.HanViet2[i].All(hv => hv != arr[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        var javi = context.JaVis.Find(simpleJavi.DbId);
                        if (javi != null)
                        {
                            result.Add(javi);
                        }
                    }
                }
            }
            return result;
            //switch (searchArgs.Option)
            //{
            //    case SearchOption.Exact:
            //        return SearchExact(key);
            //    case SearchOption.StartWith:
            //        return SearchStartWith(key);
            //    case SearchOption.EndWith:
            //        return SearchEndWith(key);
            //    case SearchOption.Contain:
            //        return SearchContain(key);
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

        }

        

        private static IEnumerable<JaVi> SearchContain(string key)
        {
            var context = DatabaseContext.Context;
            if (StringUtil.IsContainKanji(key))
            {
                return context.JaVis.Where(o => o.Word.Contains(key)).ToArray();
            }
            else
            {
                return context.JaVis.Where(o => (o.Kana!=null && o.Word.Contains(key)) || o.Kana.Contains(key)).ToArray();
            }
        }

        private static IEnumerable<JaVi> SearchEndWith(string key)
        {
            var context = DatabaseContext.Context;
            if (StringUtil.IsContainKanji(key))
            {
                return context.JaVis.Where(o => o.Word.EndsWith(key)).ToArray();
            }
            else
            {
                return context.JaVis.Where(o => (o.Kana != null && o.Word.EndsWith(key)) || o.Kana.EndsWith(key)).ToArray();
            }
            
        }

        private static IEnumerable<JaVi> SearchStartWith(string key)
        {
            var context = DatabaseContext.Context;
            if (StringUtil.IsContainKanji(key))
            {
                return context.JaVis.Where(o => o.Word.StartsWith(key)).ToArray();
            }
            else
            {
                return context.JaVis.Where(o => (o.Kana != null && o.Word.StartsWith(key)) || o.Kana.StartsWith(key)).ToArray();
            }
            
        }

        private static IEnumerable<JaVi> SearchExact(string key)
        {
            var context = DatabaseContext.Context;
            if (StringUtil.IsContainKanji(key))
            {
                return context.JaVis.Where(o => o.Word == key).ToArray();
            }
            else
            {
                return context.JaVis.Where(o => o.Word == key || o.Kana == key).ToArray();
            }
           
        }

        private static IEnumerable<JaVi> GetAll()
        {
            var context = DatabaseContext.Context;
            return context.JaVis.ToArray();
        }
    }

    class JaviHanVietCache
    {
        public List<string> HanViet { get; set; }
        public List<List<string>> HanViet2 { get; set; }
        public int DbId { get; set; }
    }
}
