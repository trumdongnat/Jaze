using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Messages
{
    public class SearchResultMessage : GenericMessage<object>
    {
        public SearchResultMessage(object content) : base(content)
        {
        }

        public SearchResultMessage(object sender, object content) : base(sender, content)
        {
        }

        public SearchResultMessage(object sender, object target, object content) : base(sender, target, content)
        {
        }
    }
}