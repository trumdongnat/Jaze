using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entity
{
    [Table("javi")]
    public class JaVi
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Kana { get; set; }
        [Column(TypeName = "ntext")]
        public string Mean { get; set; }

        //public ICollection<Kanji> Kanjis { get; set; }
    }
}