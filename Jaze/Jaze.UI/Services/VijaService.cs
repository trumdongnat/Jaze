using System.Collections.Generic;
using System.Linq;
using Jaze.Domain;
using Jaze.UI.Models;
using Newtonsoft.Json;

namespace Jaze.UI.Services
{
    public class VijaService : ServiceBase<VijaModel>
    {
        public override List<VijaModel> SearchExact(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word == key).ToList().Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> SearchStartWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word.StartsWith(key)).ToList().Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> SearchEndWith(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word.EndsWith(key)).ToList().Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> SearchContain(string key)
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.Where(o => o.Word.Contains(key)).ToList().Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override List<VijaModel> GetAll()
        {
            using (var db = new JazeDatabaseContext())
            {
                return db.Vijas.ToList().Select(entity => VijaModel.Create(entity)).ToList();
            }
        }

        public override void LoadFull(VijaModel model)
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
                    model.IsLoadFull = true;
                }
            }
        }
    }
}