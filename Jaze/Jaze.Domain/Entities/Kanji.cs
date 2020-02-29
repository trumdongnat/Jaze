using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entities
{
    [Table("kanji")]
    public class Kanji
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string HanViet { get; set; }
        public string Variant { get; set; }
        public string Onyomi { get; set; }
        public string Kunyomi { get; set; }
        public string VieMeaning { get; set; }
        public string EngMeaning { get; set; }
        public int Stroke { get; set; }
        public string StrokeOrder { get; set; }
        public int Frequence { get; set; }
        public string Similar { get; set; }
        public string Component { get; set; }
        public Level Level { get; set; }
        public Grade Grade { get; set; }

        public virtual Radical Radical { get; set; }

        //public virtual ICollection<Part> Parts { get; set; }
        public virtual ICollection<KanjiPartMap> KanjiParts { get; set; }
    }
}