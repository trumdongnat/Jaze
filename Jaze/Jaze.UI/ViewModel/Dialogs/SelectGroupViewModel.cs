using Jaze.UI.Models;
using Jaze.UI.Repository;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace Jaze.UI.ViewModel
{
    public class SelectGroupViewModel : DialogViewModelBase
    {
        private readonly IUserDataRepository _userDataRepository;
        private GroupItemModel _groupItem;
        private ObservableCollection<GroupModel> _groupCollection;

        public ObservableCollection<GroupModel> GroupCollection
        {
            get => _groupCollection;
            set => SetProperty(ref _groupCollection, value);
        }

        private string _newGroupName;

        public string NewGroupName
        {
            get => _newGroupName;
            set => SetProperty(ref _newGroupName, value);
        }

        public GroupModel SelectedGroup { get; private set; }

        private DelegateCommand _addToNewGroupCommand;

        public DelegateCommand AddToNewGroupCommand =>
            _addToNewGroupCommand ?? (_addToNewGroupCommand = new DelegateCommand(ExecuteAddToNewGroupCommand, CanExecuteAddToNewGroupCommand).ObservesProperty((() => NewGroupName)));

        private async void ExecuteAddToNewGroupCommand()
        {
            int id = await _userDataRepository.AddGroup(NewGroupName);
            await _userDataRepository.AddWord(id, _groupItem.Type, _groupItem.WordId);
            SelectedGroup = await _userDataRepository.GetGroup(id);
            RaiseCloseEvent();
        }

        private bool CanExecuteAddToNewGroupCommand()
        {
            return !string.IsNullOrWhiteSpace(NewGroupName);
        }

        private DelegateCommand<GroupModel> _addToGroupCommand;

        public DelegateCommand<GroupModel> AddToGroupCommand =>
            _addToGroupCommand ?? (_addToGroupCommand = new DelegateCommand<GroupModel>(ExecuteAddToGroupCommand));

        private async void ExecuteAddToGroupCommand(GroupModel group)
        {
            await _userDataRepository.AddWord(group.Id, _groupItem.Type, _groupItem.WordId);
            SelectedGroup = group;
            RaiseCloseEvent();
        }

        private void RaiseCloseEvent()
        {
            var parameters = new DialogParameters();
            parameters.Add("SelectedGroup", SelectedGroup);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            _groupItem = parameters.GetValue<GroupItemModel>("GroupItem");
            Init();
        }

        public override event Action<IDialogResult> RequestClose;

        private async void Init()
        {
            var groups = await _userDataRepository.GetListGroup();
            GroupCollection = new ObservableCollection<GroupModel>(groups);
        }

        public SelectGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
            Title = "Select Group";
        }
    }
}