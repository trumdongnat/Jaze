using System.Collections.Generic;

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