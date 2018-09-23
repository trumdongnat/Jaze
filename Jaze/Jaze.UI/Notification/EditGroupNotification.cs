using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.UI.Models;
using Prism.Interactivity.InteractionRequest;

namespace Jaze.UI.Notification
{
    public class EditGroupNotification : IConfirmation
    {
        public string Title { get; set; }
        public object Content { get; set; }
        public bool Confirmed { get; set; }
        public GroupModel Group { get; set; }
    }
}