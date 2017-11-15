using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class PartModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Word { get; set; }
        public int Stroke { get; set; }

        public ICollection<KanjiModel> Kanjis { get; set; }

        public static PartModel Create(Part entity)
        {
            return new PartModel
            {
                Id = entity.Id,
                Value = entity.Value,
                Word = entity.Word,
                Stroke = entity.Stroke,
            };
        }
    }
}