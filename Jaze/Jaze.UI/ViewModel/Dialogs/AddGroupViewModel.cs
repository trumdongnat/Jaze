using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.UI.Repository;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Services.Dialogs;

namespace Jaze.UI.ViewModel
{
    public class AddGroupViewModel : DialogViewModelBase
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
            RaiseCloseEvent(group);
        }

        

        private bool CanExecuteAddGroupCommand()
        {
            return !string.IsNullOrWhiteSpace(GroupName);
        }

        private void RaiseCloseEvent(Models.GroupModel group)
        {
            var parameters = new DialogParameters();
            parameters.Add("Group", group);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        public override event Action<IDialogResult> RequestClose;
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        public AddGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }
    }
}