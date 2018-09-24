using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.UI.Models;
using Prism.Interactivity.InteractionRequest;

namespace Jaze.UI.Notification
{
    public class AddToGroupNotification : INotification
    {
        public string Title { get; set; }
        public object Content { get; set; }
        public List<GroupItemModel> Items { get; set; }
    }
}