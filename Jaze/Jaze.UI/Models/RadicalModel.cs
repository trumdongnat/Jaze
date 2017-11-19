using System.Collections.Generic;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class RadicalModel
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string HanViet { get; set; }
        public string Meaning { get; set; }
        public int Stroke { get; set; }

        public ICollection<KanjiModel> Kanjis { get; set; }

        public static RadicalModel Create(Radical entity)
        {
            return new RadicalModel
            {
                Id = entity.Id,
                Word = entity.Word,
                HanViet = entity.HanViet,
                Meaning = entity.Meaning,
                Stroke = entity.Stroke,
            };
        }
    }
}