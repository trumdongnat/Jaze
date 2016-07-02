using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.DAO.Model
{
    [Table("grade")]
    public class Grade
    {
        public Grade()
        {
            Kanjis = new HashSet<Kanji>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Kanji> Kanjis { get; set; }
    }
}