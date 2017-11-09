using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entity
{
    [Table("vija")]
    public class ViJa
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Mean { get; set; }
    }
}
