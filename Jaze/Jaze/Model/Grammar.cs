using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Model
{
    [Table("grammar")]
    public class Grammar
    {
        public int Id { get; set; }
        public string Struct { get; set; }
        public string Meaning { get; set; }
        public string Detail { get; set; }


        public virtual Level Level { get; set; }
    }
}
