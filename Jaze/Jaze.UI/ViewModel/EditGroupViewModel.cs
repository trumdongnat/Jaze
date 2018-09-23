using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private EditGroupNotification _editGroupNotification;

        public INotification Notification
        {
            get => _editGroupNotification; set
            {
                if (value is EditGroupNotification notification)
                {
                    _editGroupNotification = notification;
                    GroupName = notification.Group.Name;
                }
            }
        }

        public Action FinishInteraction { get; set; }

        public EditGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }
    }
}