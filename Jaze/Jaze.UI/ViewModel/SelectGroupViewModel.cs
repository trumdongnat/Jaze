using System;
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
    public class SelectGroupViewModel : ViewModelBase, IInteractionRequestAware
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

        private DelegateCommand _addToNewGroupCommand;

        public DelegateCommand AddToNewGroupCommand =>
            _addToNewGroupCommand ?? (_addToNewGroupCommand = new DelegateCommand(ExecuteAddToNewGroupCommand, CanExecuteAddToNewGroupCommand).ObservesProperty((() => NewGroupName)));

        private async void ExecuteAddToNewGroupCommand()
        {
            int id = await _userDataRepository.AddGroup(NewGroupName);
            await _userDataRepository.AddWord(id, _groupItem.Type, _groupItem.WordId);
            _selectGroupNotification.SelectedGroup = await _userDataRepository.GetGroup(id);
            FinishInteraction.Invoke();
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
            _selectGroupNotification.SelectedGroup = group;
            FinishInteraction.Invoke();
        }

        private ISelectGroupNotification _selectGroupNotification;

        public INotification Notification
        {
            get => _selectGroupNotification;
            set
            {
                if (value is ISelectGroupNotification selectGroupNotification)
                {
                    _selectGroupNotification = selectGroupNotification;
                    _groupItem = _selectGroupNotification.GroupItem;
                    Init();
                }
            }
        }

        private async void Init()
        {
            var groups = await _userDataRepository.GetListGroup();
            GroupCollection = new ObservableCollection<GroupModel>(groups);
        }

        public Action FinishInteraction { get; set; }

        public SelectGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }
    }
}