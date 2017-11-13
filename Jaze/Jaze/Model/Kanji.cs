using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Model
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

        public Level Level { get; set; }
        public Grade Grade { get; set; }
        public virtual Radical Radical { get; set; }
        public virtual ICollection<Part> Parts { get; set; }        
    }

    public enum Grade
    {
        Unknown = 0,
        Grade1 = 1,
        Grade2 = 2,
        Grade3 = 3,
        Grade4 = 4,
        Grade5 = 5,
        Grade6 = 6
    }

    public enum Level
    {
        Unknown = 0,
        N1 = 1,
        N2 = 2,
        N3 = 3,
        N4 = 5,
        N5 = 5,
        Basic = 6,
    }
}