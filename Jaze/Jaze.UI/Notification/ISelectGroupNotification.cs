using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Entities;
using Jaze.UI.Models;
using Prism.Interactivity.InteractionRequest;

namespace Jaze.UI.Notification
{
    public interface ISelectGroupNotification : INotification
    {
        GroupItemModel GroupItem { get; set; }
        GroupModel SelectedGroup { get; set; }
    }
}