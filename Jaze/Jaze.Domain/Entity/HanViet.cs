using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entity
{
    [Table("hanviet")]
    public class HanViet
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Reading { get; set; }
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
    }
}