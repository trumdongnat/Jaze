using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Notification
{
    public class ShowKanjiPartNofitication : IShowKanjiPartNotification
    {
        public string Title { get; set; }
        public object Content { get; set; }
        public List<string> Parts { get; set; }
    }
}