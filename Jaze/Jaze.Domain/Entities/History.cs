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
    public class History
    {
        [Key, Column(Order = 0)]
        public DictionaryType Type { get; set; }

        [Key, Column(Order = 1)]
        public int Id { get; set; }

        public DateTime LastTime { get; set; }
    }
}