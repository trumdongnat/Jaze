using System.Collections.Generic;
using System.Linq;
using Jaze.Domain;
using Jaze.UI.Models;

namespace Jaze.UI.Services
{
    public class KanjiSearcher : ServiceBase<KanjiModel>
    {
        //public static IEnumerable<Kanji> Search(SearchArgs searchArgs)
        //{
        //    var key = searchArgs.SearchKey;
        //    //
        //    if (string.IsNullOrWhiteSpace(key))
        //    {
        //        return GetAll();
        //    }
        //    //if search key contain multi kanji
        //    var arr = StringUtil.FilterCharsInString(key, CharSet.Kanji);
        //    if (arr.Count>0)
        //    {
        //        return LoadKanji(arr);
        //    }
        //    //if search key is vietnamese sentence
        //    if (key.Contains(" "))
        //    {
        //        return SearchVietNameseSentence(key);
        //    }
        //    //other case
        //    switch (searchArgs.Option)
        //    {
        //        case SearchOption.Exact:
        //            return SearchExact(key);
        //        case SearchOption.StartWith:
        //            return SearchStartWith(key);
        //        case SearchOption.EndWith:
        //            return SearchEndWith(key);
        //        case SearchOption.Contain:
        //            return SearchContain(key);
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        //private static IEnumerable<Kanji> SearchVietNameseSentence(string key)
        //{
        //    var arr = key.Split(' ');
        //    var db = JazeDatabaseContext.Context;
        //    var list = new List<Kanji>();
        //    foreach (var s in arr)
        //    {
        //        var kanjis = db.Kanjis.Where(k => k.HanViet == s);
        //        foreach (var kanji in kanjis)
        //        {
        //            if (!list.Contains(kanji))
        //            {
        //                list.Add(kanji);
        //            }
        //        }
        //    }
        //    return list;
        //}

        //private static IEnumerable<Kanji> LoadKanji(IList<char> arr)
        //{
        //    var db = JazeDatabaseContext.Context;
        //    return arr.Select(c => "" + c).Select(s => db.Kanjis.FirstOrDefault(kanji => kanji.Word == s)).ToList();
        //}

        //private static IEnumerable<Kanji> SearchContain(string key)
        //{
        //    var db = JazeDatabaseContext.Context;
        //    return db.Kanjis.Where(kanji => kanji.HanViet.Contains(key)).ToArray();
        //}

        //private static IEnumerable<Kanji> SearchEndWith(string key)
        //{
        //    var db = JazeDatabaseContext.Context;
        //    return db.Kanjis.Where(kanji => kanji.HanViet.EndsWith(key)).ToArray();
        //}

        //private static IEnumerable<Kanji> SearchStartWith(string key)
        //{
        //    var db = JazeDatabaseContext.Context;
        //    return db.Kanjis.Where(kanji => kanji.HanViet.StartsWith(key)).ToArray();
        //}

        //private static IEnumerable<Kanji> SearchExact(string key)
        //{
        //    var db = JazeDatabaseContext.Context;
        //    //at stat
        //    var keyStart = key + ",";
        //    //at middle
        //    var keyMiddle = "," + key + ",";
        //    //at end
        //    var keyEnd = "," + key;
        //    return db.Kanjis.Where(kanji => kanji.HanViet == key || kanji.HanViet.StartsWith(keyStart) || kanji.HanViet.Contains(keyMiddle) || kanji.HanViet.EndsWith(keyEnd)).ToArray();
        //}

        //private static IEnumerable<Kanji> GetAll()
        //{
        //    var db = JazeDatabaseContext.Context;
        //    return db.Kanjis.ToArray();
        //}
        public override List<KanjiModel> SearchExact(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                //at stat
                var keyStart = key + ",";
                //at middle
                var keyMiddle = "," + key + ",";
                //at end
                var keyEnd = "," + key;
                return db.Kanjis.Where(kanji => kanji.HanViet == key || kanji.HanViet.StartsWith(keyStart) || kanji.HanViet.Contains(keyMiddle) || kanji.HanViet.EndsWith(keyEnd)).Select(entity => KanjiModel.Create(entity)).ToList();
            }
        }

        public override List<KanjiModel> SearchStartWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Kanjis.Where(kanji => kanji.HanViet.StartsWith(key)).Select(entity => KanjiModel.Create(entity)).ToList();
            }
        }

        public override List<KanjiModel> SearchEndWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Kanjis.Where(kanji => kanji.HanViet.EndsWith(key)).Select(entity => KanjiModel.Create(entity)).ToList();
            }
        }

        public override List<KanjiModel> SearchContain(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Kanjis.Where(kanji => kanji.HanViet.Contains(key)).Select(entity => KanjiModel.Create(entity)).ToList();
            }
        }

        public override List<KanjiModel> GetAll()
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Kanjis.Select(entity => KanjiModel.Create(entity)).ToList();
            }
        }
    }
}