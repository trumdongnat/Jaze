using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;

namespace Jaze.Domain.Entities
{
    public class GroupItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DictionaryType Type { get; set; }
        public int WordId { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}