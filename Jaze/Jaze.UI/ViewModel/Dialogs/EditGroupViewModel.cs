using Jaze.UI.Models;
using Jaze.UI.Repository;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jaze.UI.ViewModel
{
    public class EditGroupViewModel : DialogViewModelBase
    {
        private readonly IUserDataRepository _userDataRepository;
        private string _groupName;

        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        private GroupModel _group;

        public GroupModel Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
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
            await _userDataRepository.ChangeGroupName(Group.Id, GroupName);
            Group.Name = GroupName;
            RaiseCloseEvent();
        }

        private bool CanExecuteEditGroupCommand()
        {
            return !string.IsNullOrWhiteSpace(GroupName) && GroupName != Group.Name;
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
                    await _userDataRepository.RemoveWord(Group.Id, item.Type, item.WordId);
                    ItemCollection.Remove(item);
                }
            }
        }

        private void RaiseCloseEvent()
        {
            var parameters = new DialogParameters();
            parameters.Add("Group", Group);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        public override event Action<IDialogResult> RequestClose;

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

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Group = parameters.GetValue<GroupModel>("Group");
            SetGroup(Group);
        }

        public Action FinishInteraction { get; set; }

        public EditGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }
    }
}