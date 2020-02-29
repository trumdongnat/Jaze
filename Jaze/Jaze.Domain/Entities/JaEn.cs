using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entities
{
    [Table("jaen")]
    public class JaEn
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Kana { get; set; }
        public string Mean { get; set; }
    }
}
