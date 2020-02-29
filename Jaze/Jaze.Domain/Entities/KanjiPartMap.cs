using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.Domain.Entities
{
    [Table("Kanji_Part")]
    public class KanjiPartMap
    {
        [Key, Column(Order = 0)]
        public int KanjiId { get; set; }

        [Key, Column(Order = 1)]
        public int PartId { get; set; }

        [ForeignKey("KanjiId")]
        public virtual Kanji Kanji { get; set; }

        [ForeignKey("PartId")]
        public virtual Part Part { get; set; }
    }
}