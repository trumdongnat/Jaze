using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.DAO.Model
{
    [Table("vija")]
    public class ViJa
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Mean { get; set; }
    }
}
