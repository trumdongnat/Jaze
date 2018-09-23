using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Entities;
using Jaze.UI.Models;

namespace Jaze.UI.Notification
{
    public class SelectGroupNotification : ISelectGroupNotification
    {
        public string Title { get; set; }
        public object Content { get; set; }
        public GroupItemModel GroupItem { get; set; }
        public GroupModel SelectedGroup { get; set; }
    }
}