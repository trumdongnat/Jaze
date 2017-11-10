using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entities
{
    [Table("radical")]
    public class Radical
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string HanViet { get; set; }
        public string Meaning { get; set; }
        public int Stroke { get; set; }

        public virtual ICollection<Kanji> Kanjis { get; set; }
    }
}