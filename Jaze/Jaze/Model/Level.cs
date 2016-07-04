using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Model
{
    [Table("level")]
    public class Level
    {
        public Level()
        {
            Kanjis = new HashSet<Kanji>();
            Grammars = new HashSet<Grammar>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Kanji> Kanjis { get; set; }
        public virtual ICollection<Grammar> Grammars { get; set; }
    }
}