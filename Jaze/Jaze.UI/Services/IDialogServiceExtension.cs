using Jaze.UI.Models;
using Jaze.UI.Views;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Services
{
    public static class IDialogServiceExtension
    {
        public static void ShowKanjiPartDialog(this IDialogService dialogService, List<string> parts, Action<IDialogResult> callback = default)
        {
            var parameters = new DialogParameters();
            parameters.Add("Parts", parts);
            dialogService.ShowDialog(nameof(KanjiPart), parameters, callback);
        }

        public static void ShowSelectGroupDialog(this IDialogService dialogService, GroupItemModel groupItem, Action<IDialogResult> callback = default)
        {
            var parameters = new DialogParameters();
            parameters.Add("GroupItem", groupItem);
            dialogService.ShowDialog(nameof(SelectGroupView), parameters, callback);
        }

        public static void ShowAddGroupDialog(this IDialogService dialogService, Action<IDialogResult> callback = default)
        {
            dialogService.ShowDialog(nameof(AddGroupView), null, callback);
        }

        public static void ShowAddToGroupDialog(this IDialogService dialogService, List<GroupItemModel> items, Action<IDialogResult> callback = default)
        {
            var parameters = new DialogParameters();
            parameters.Add("Items", items);
            dialogService.ShowDialog(nameof(AddToGroupView), parameters, callback);
        }

        public static void ShowEditGroupDialog(this IDialogService dialogService, GroupModel group, Action<IDialogResult> callback = default)
        {
            var parameters = new DialogParameters();
            parameters.Add("Group", group);
            dialogService.ShowDialog(nameof(EditGroupView), parameters, callback);
        }
    }
}