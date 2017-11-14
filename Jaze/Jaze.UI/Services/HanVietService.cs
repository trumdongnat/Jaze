using Jaze.Domain;
using Jaze.UI.Models;
using System.Collections.Generic;
using System.Linq;

namespace Jaze.UI.Services
{
    public class HanVietService : ServiceBase<HanVietModel>
    {
        //public static IEnumerable<HanViet> Search(SearchArgs searchArgs)
        //{
        //    var key = searchArgs.SearchKey;

        //    if (string.IsNullOrWhiteSpace(key))
        //    {
        //        //return GetAll();
        //        return null;
        //    }

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

        //private static IEnumerable<HanViet> SearchContain(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.HanViets.Where(hv => hv.Word.Contains(key) || hv.Reading.Contains(key)).ToArray();
        //}

        //private static IEnumerable<HanViet> SearchEndWith(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.HanViets.Where(hv => hv.Word.EndsWith(key) || hv.Reading.EndsWith(key)).ToArray();
        //}

        //private static IEnumerable<HanViet> SearchStartWith(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.HanViets.Where(hv => hv.Word.StartsWith(key) || hv.Reading.StartsWith(key)).ToArray();
        //}

        //private static IEnumerable<HanViet> SearchExact(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    //at stat
        //    var keyStart = key + ", ";
        //    //at middle
        //    var keyMiddle = ", " + key + ",";
        //    //at end
        //    var keyEnd = ", " + key;
        //    return context.HanViets.Where(hv => hv.Word == key || hv.Reading == key || hv.Reading.StartsWith(keyStart) || hv.Reading.Contains(keyMiddle) || hv.Reading.EndsWith(keyEnd)).ToArray();
        //}

        //private static IEnumerable<HanViet> GetAll()
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.HanViets.ToArray();
        //}
        public override List<HanVietModel> SearchExact(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                //at stat
                var keyStart = key + ", ";
                //at middle
                var keyMiddle = ", " + key + ",";
                //at end
                var keyEnd = ", " + key;
                return db.HanViets.Where(hv => hv.Word == key || hv.Reading == key || hv.Reading.StartsWith(keyStart) || hv.Reading.Contains(keyMiddle) || hv.Reading.EndsWith(keyEnd))
                    .Select(hv => HanVietModel.Create(hv)).ToList();
            }
        }

        public override List<HanVietModel> SearchStartWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.HanViets.Where(hv => hv.Word.StartsWith(key) || hv.Reading.StartsWith(key))
                    .Select(hv => HanVietModel.Create(hv)).ToList();
            }
        }

        public override List<HanVietModel> SearchEndWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.HanViets.Where(hv => hv.Word.EndsWith(key) || hv.Reading.EndsWith(key))
                    .Select(hv => HanVietModel.Create(hv)).ToList();
            }
        }

        public override List<HanVietModel> SearchContain(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.HanViets.Where(hv => hv.Word.Contains(key) || hv.Reading.Contains(key))
                    .Select(hv => HanVietModel.Create(hv)).ToList();
            }
        }

        public override List<HanVietModel> GetAll()
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.HanViets.Select(hv => HanVietModel.Create(hv)).ToList();
            }
        }
    }
}