using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jaze.Domain;
using Jaze.UI.Models;
using Jaze.UI.Util;
using Newtonsoft.Json;

namespace Jaze.UI.Services
{
    public class JaviService : ServiceBase<JaviModel>
    {
        private const string HanvietCacheFile = "jv_hv";
        private static bool _isLoadingCache = false;
        private static List<JaviHanVietCache> _javiHanVietCaches;

        static JaviService()
        {
            LoadCachesAsync();
        }

        private static async void LoadCachesAsync()
        {
            await Task.Run(() => LoadCaches());
        }

        private static void LoadCaches()
        {
            _isLoadingCache = true;
            _javiHanVietCaches = new List<JaviHanVietCache>();
            string cacheFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Caches");
            var cachePath = System.IO.Path.Combine(cacheFolder, HanvietCacheFile);
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
                using (var db = new JazeDatabaseContext())
                {
                    var hvDic = db.Kanjis.ToDictionary(kanji => kanji.Word, kanji => kanji.HanViet);
                    var javis = db.JaVis.Select(javi => new { javi.Id, javi.Word }).ToList();
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
            }
            foreach (var simpleJavi in _javiHanVietCaches)
            {
                simpleJavi.HanViet2 = new List<List<string>>();
                foreach (var s in simpleJavi.HanViet)
                {
                    simpleJavi.HanViet2.Add(s.Split(',').ToList());
                }
            }
            _isLoadingCache = false;
        }

        private List<JaviModel> SearchHanViet(SearchArgs searchArgs)
        {
            if (_isLoadingCache) { return new List<JaviModel>(); }
            var key = searchArgs.SearchKey;
            var arr = key.Split(' ');
            var kanjiOfWord = arr.Length;
            var result = new List<JaviModel>();
            using (var db = new JazeDatabaseContext())
            {
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
                            var javi = db.JaVis.Find(simpleJavi.DbId);
                            if (javi != null)
                            {
                                result.Add(JaviModel.Create(javi));
                            }
                        }
                    }
                }
                return result;
            }
        }

        public override JaviModel Get(int id)
        {
            using (var db = new JazeDatabaseContext())
            {
                var entity = db.JaVis.Find(id);
                if (entity != null)
                {
                    return JaviModel.Create(entity);
                }

                return null;
            }
        }

        public override List<JaviModel> Search(SearchArgs searchArgs)
        {
            var rawKey = searchArgs.SearchKey;

            if (string.IsNullOrWhiteSpace(rawKey))
            {
                return new List<JaviModel>();
            }
            var key = rawKey.Contains("-") ? StringUtil.ConvertRomaji2Katakana(rawKey) : StringUtil.ConvertRomaji2Hiragana(rawKey);
            List<JaviModel> resultJv = new List<JaviModel>();
            List<JaviModel> resultHv = new List<JaviModel>();
            if (StringUtil.IsJapanese(key))
            {
                resultJv = base.Search(new SearchArgs(key, searchArgs.Option));
            }

            if (rawKey.Split(' ').All(StringUtil.IsVietnameseWord))
            {
                resultHv = SearchHanViet(searchArgs);
            }
            return resultJv.Union(resultHv).ToList();
        }

        public override List<JaviModel> SearchExact(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                if (StringUtil.IsContainKanji(key))
                {
                    return db.JaVis.Where(o => o.Word == key).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
                else
                {
                    return db.JaVis.Where(o => o.Word == key || o.Kana == key).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
            }
        }

        public override List<JaviModel> SearchStartWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                if (StringUtil.IsContainKanji(key))
                {
                    return db.JaVis.Where(o => o.Word.StartsWith(key)).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
                else
                {
                    return db.JaVis.Where(o => o.Word.StartsWith(key) || o.Kana.StartsWith(key)).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
            }
        }

        public override List<JaviModel> SearchEndWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                if (StringUtil.IsContainKanji(key))
                {
                    return db.JaVis.Where(o => o.Word.EndsWith(key)).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
                else
                {
                    return db.JaVis.Where(o => o.Word.EndsWith(key) || o.Kana.EndsWith(key)).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
            }
        }

        public override List<JaviModel> SearchContain(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                if (StringUtil.IsContainKanji(key))
                {
                    return db.JaVis.Where(o => o.Word.Contains(key)).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
                else
                {
                    return db.JaVis.Where(o => o.Word.Contains(key) || o.Kana.Contains(key)).ToList().Select(o => JaviModel.Create(o)).ToList();
                }
            }
        }

        public override List<JaviModel> GetAll()
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.JaVis.ToList().Select(o => JaviModel.Create(o)).ToList();
            }
        }

        public override void LoadFull(JaviModel model)
        {
            if (!model.IsLoadFull)
            {
                using (var db = new JazeDatabaseContext())
                {
                    model.Means = JsonConvert.DeserializeObject<List<WordMean>>(model.MeanText);
                    foreach (var mean in model.Means)
                    {
                        if (mean.ExampleIds != null)
                        {
                            mean.Examples = new List<ExampleModel>();
                            foreach (var exampleId in mean.ExampleIds)
                            {
                                var example = db.JaViExamples.Find(exampleId);
                                if (example != null)
                                {
                                    mean.Examples.Add(ExampleModel.Create(example));
                                }
                            }
                        }
                    }
                    //fetch kanji in word
                    model.Kanjis = new List<KanjiModel>();
                    var kanjis = StringUtil.FilterCharsInString(model.Word, CharSet.Kanji);
                    foreach (var c in kanjis)
                    {
                        var kanji = c.ToString();
                        var kanjiEntity = db.Kanjis.FirstOrDefault(e => e.Word == kanji);
                        if (kanjiEntity != null)
                        {
                            model.Kanjis.Add(KanjiModel.Create(kanjiEntity));
                        }
                        else
                        {
                            model.Kanjis.Add(new KanjiModel { Word = kanji });
                        }
                    }
                    model.IsLoadFull = true;
                }
            }
        }
    }

    internal class JaviHanVietCache
    {
        public List<string> HanViet { get; set; }
        public List<List<string>> HanViet2 { get; set; }
        public int DbId { get; set; }
    }
}