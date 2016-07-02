using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.DAO.Model
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
