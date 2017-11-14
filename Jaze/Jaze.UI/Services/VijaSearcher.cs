using System.Collections.Generic;
using System.Linq;
using Jaze.Domain;
using Jaze.UI.Models;

namespace Jaze.UI.Services
{
    public class VijaSearcher : ServiceBase<VijaModel>
    {
        public override List<VijaModel> SearchExact(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word == key).Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> SearchStartWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word == key).Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> SearchEndWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word.EndsWith(key)).Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> SearchContain(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word.Contains(key)).Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> GetAll()
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Select(entity => VijaModel.Create(entity)).ToList();
            }
        }
    }
}