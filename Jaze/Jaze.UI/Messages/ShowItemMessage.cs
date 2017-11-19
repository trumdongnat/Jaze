using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Messages
{
    public class ShowItemMessage
    {
        public object Item { get; set; }

        public ShowItemMessage(object item)
        {
            Item = item;
        }
    }
}