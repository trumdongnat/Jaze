using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.UI.Models;
using Jaze.UI.Notification;
using Jaze.UI.Repository;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Jaze.UI.ViewModel
{
    public class EditGroupViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly IUserDataRepository _userDataRepository;
        private string _groupName;

        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        private ObservableCollection<GroupItemModel> _itemCollection = new ObservableCollection<GroupItemModel>();

        public ObservableCollection<GroupItemModel> ItemCollection
        {
            get => _itemCollection;
            set => SetProperty(ref _itemCollection, value);
        }

        private DelegateCommand _editGroupCommand;

        public DelegateCommand EditGroupCommand =>
            _editGroupCommand ?? (_editGroupCommand = new DelegateCommand(ExecuteEditGroupCommand, CanExecuteEditGroupCommand).ObservesProperty(() => GroupName));

        private async void ExecuteEditGroupCommand()
        {
            await _userDataRepository.ChangeGroupName(_editGroupNotification.Group.Id, GroupName);
            _editGroupNotification.Group.Name = GroupName;
            _editGroupNotification.Confirmed = true;
            FinishInteraction.Invoke();
        }

        private bool CanExecuteEditGroupCommand()
        {
            return !string.IsNullOrWhiteSpace(GroupName) && GroupName != _editGroupNotification.Group.Name;
        }

        private DelegateCommand<IList> _deleteItemsCommand;

        public DelegateCommand<IList> DeleteItemsCommand =>
            _deleteItemsCommand ?? (_deleteItemsCommand = new DelegateCommand<IList>(ExecuteDeleteItemsCommand));

        private async void ExecuteDeleteItemsCommand(IList parameter)
        {
            if (parameter != null)
            {
                var items = parameter.OfType<GroupItemModel>().ToArray();
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    await _userDataRepository.RemoveWord(_editGroupNotification.Group.Id, item.Type, item.WordId);
                    ItemCollection.Remove(item);
                }
            }
        }

        private EditGroupNotification _editGroupNotification;

        public INotification Notification
        {
            get => _editGroupNotification; set
            {
                if (value is EditGroupNotification notification)
                {
                    _editGroupNotification = notification;
                    SetGroup(notification.Group);
                }
            }
        }

        private async void SetGroup(GroupModel group)
        {
            ItemCollection.Clear();
            GroupName = group.Name;
            if (!group.IsLoadFull || group.Items.Any(item => !item.IsLoadFull))
            {
                await _userDataRepository.LoadFull(group);
            }

            ItemCollection = group.Items;
        }

        public Action FinishInteraction { get; set; }

        public EditGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }
    }
}