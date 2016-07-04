using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Model
{
    [Table("vija")]
    public class ViJa
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Mean { get; set; }
    }
}
