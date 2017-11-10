using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.Domain.Entities
{
    [Table("jaenExam")]
    public class JaEnExample
    {
        public int Id { get; set; }

        [Column("ja")]
        public string Japanese { get; set; }

        [Column("en")]
        public string English { get; set; }
        
    }
}