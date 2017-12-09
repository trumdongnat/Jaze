using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Messages
{
    public class ShowPartsMessage
    {
        public ShowPartsMessage(List<string> parts)
        {
            Parts = parts;
        }

        public List<string> Parts { get; set; }
    }
}