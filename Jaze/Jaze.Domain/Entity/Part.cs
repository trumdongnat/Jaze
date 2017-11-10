using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entity
{
    [Table("part")]
    public class Part
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Word { get; set; }
        public int Stroke { get; set; }

        public virtual ICollection<Kanji> Kanjis { get; set; }
    }
}