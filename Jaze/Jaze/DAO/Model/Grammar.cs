using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.DAO.Model
{
    [Table("grammar")]
    public class Grammar
    {
        public int Id { get; set; }
        public string Struct { get; set; }
        public string Meaning { get; set; }
        public string Detail { get; set; }


        public Level Level { get; set; }
    }
}
