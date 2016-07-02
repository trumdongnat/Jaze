using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.DAO.Model
{
    [Table("javi")]
    public class JaVi
    {
        //public JaVi()
        //{
        //    Kanjis = new HashSet<Kanji>();
        //}

        public int Id { get; set; }
        public string Word { get; set; }
        public string Kana { get; set; }
        [Column(TypeName = "ntext")]
        public string Mean { get; set; }

        //public ICollection<Kanji> Kanjis { get; set; }
    }
}