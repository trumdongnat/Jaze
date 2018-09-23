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
    public class AddGroupViewModel : ViewModelBase, IInteractionRequestAware
    {
        private string _groupName;
        private readonly IUserDataRepository _userDataRepository;

        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        private DelegateCommand _addGroupCommand;

        public DelegateCommand AddGroupCommand =>
            _addGroupCommand ?? (_addGroupCommand = new DelegateCommand(ExecuteAddGroupCommand, CanExecuteAddGroupCommand).ObservesProperty(() => GroupName));

        private async void ExecuteAddGroupCommand()
        {
            var id = await _userDataRepository.AddGroup(GroupName);
            var group = await _userDataRepository.GetGroup(id);
            _addGroupNotification.GroupModel = group;
            _addGroupNotification.Confirmed = true;
            FinishInteraction.Invoke();
        }

        private bool CanExecuteAddGroupCommand()
        {
            return !string.IsNullOrWhiteSpace(GroupName);
        }

        private AddGroupNotification _addGroupNotification;

        public INotification Notification
        {
            get => _addGroupNotification; set
            {
                if (value is AddGroupNotification notification)
                {
                    _addGroupNotification = notification;
                }
            }
        }

        public Action FinishInteraction { get; set; }

        public AddGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }
    }
}