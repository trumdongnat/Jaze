using System.Collections.Generic;
using System.Linq;
using Jaze.Domain;
using Jaze.UI.Models;
using Jaze.UI.Util;

namespace Jaze.UI.Services
{
    public class GrammarService : ServiceBase<GrammarModel>
    {
        //public static IEnumerable<Grammar> Search(SearchArgs searchArgs)
        //{
        //    var key = searchArgs.SearchKey;

        //    if (string.IsNullOrWhiteSpace(key))
        //    {
        //        return GetAll();
        //    }

        //    //switch (searchArgs.Option)
        //    //{
        //    //    case SearchOption.Exact:
        //    //        return SearchExact(key);
        //    //    case SearchOption.StartWith:
        //    //        return SearchStartWith(key);
        //    //    case SearchOption.EndWith:
        //    //        return SearchEndWith(key);
        //    //    case SearchOption.Contain:
        //    //        return SearchContain(key);
        //    //    default:
        //    //        throw new ArgumentOutOfRangeException();
        //    //}
        //    return SearchContain(key);
        //}

        //private static IEnumerable<Grammar> SearchContain(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    var hirakey = StringUtil.ConvertRomaji2Hiragana(key);
        //    return context.Grammars.Where(o => o.Struct.Contains(hirakey) || o.Meaning.Contains(key)).ToArray();
        //}

        //private static IEnumerable<Grammar> SearchEndWith(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.Grammars.Where(o => o.Struct.EndsWith(key) || o.Meaning.EndsWith(key)).ToArray();
        //}

        //private static IEnumerable<Grammar> SearchStartWith(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.Grammars.Where(o => o.Struct.StartsWith(key) || o.Meaning.StartsWith(key)).ToArray();
        //}

        //private static IEnumerable<Grammar> SearchExact(string key)
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.Grammars.Where(o => o.Struct == key || o.Meaning == key).ToArray();
        //}

        //private static IEnumerable<Grammar> GetAll()
        //{
        //    var context = JazeDatabaseContext.Context;
        //    return context.Grammars.ToArray();
        //}

        public override List<GrammarModel> SearchExact(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Grammars.Where(o => o.Struct == key || o.Meaning == key).ToList().Select(o => GrammarModel.Create(o))
                    .ToList();
            }
        }

        public override List<GrammarModel> SearchStartWith(string key)
        {
            throw new System.NotImplementedException();
        }

        public override List<GrammarModel> SearchEndWith(string key)
        {
            throw new System.NotImplementedException();
        }

        public override List<GrammarModel> SearchContain(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                var hirakey = StringUtil.ConvertRomaji2Hiragana(key);
                return db.Grammars.Where(o => o.Struct.Contains(hirakey) || o.Meaning.Contains(key)).ToList().Select(o => GrammarModel.Create(o))
                    .ToList();
            }
        }

        public override List<GrammarModel> GetAll()
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Grammars.ToList().Select(o => GrammarModel.Create(o)).ToList();
            }
        }
    }
}