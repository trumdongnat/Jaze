using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entity
{
    [Table("kanji")]
    public class Kanji
    {
        public Kanji()
        {
            Stroke = 0;
            Parts = new HashSet<Part>();
        }

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

        public virtual Level Level { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual Radical Radical { get; set; }
        public virtual ICollection<Part> Parts { get; set; }        
    }
}