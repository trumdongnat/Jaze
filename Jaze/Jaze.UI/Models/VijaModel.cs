using System.Collections.Generic;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class VijaModel : ModelBase
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string MeanText { get; set; }
        public List<WordMean> Means { get; set; }

        public static VijaModel Create(ViJa entity)
        {
            return new VijaModel
            {
                Id = entity.Id,
                Word = entity.Word,
                MeanText = entity.Mean,
            };
        }
    }
}