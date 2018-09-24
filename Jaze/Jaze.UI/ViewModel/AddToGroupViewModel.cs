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
    public class AddToGroupViewModel : ViewModelBase, IInteractionRequestAware
    {
        private readonly IUserDataRepository _userDataRepository;

        private ObservableCollection<GroupModel> _groupCollection = new ObservableCollection<GroupModel>();

        public ObservableCollection<GroupModel> GroupCollection
        {
            get => _groupCollection;
            set => SetProperty(ref _groupCollection, value);
        }

        private GroupModel _selectedGroup;

        public GroupModel SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);
        }

        private ObservableCollection<GroupItemModel> _selectedItemCollection = new ObservableCollection<GroupItemModel>();

        public ObservableCollection<GroupItemModel> SelectedItemCollection
        {
            get => _selectedItemCollection;
            set => SetProperty(ref _selectedItemCollection, value);
        }

        private ObservableCollection<GroupItemModel> _unselectedItemCollection = new ObservableCollection<GroupItemModel>();

        public ObservableCollection<GroupItemModel> UnselectedItemCollection
        {
            get => _unselectedItemCollection;
            set => SetProperty(ref _unselectedItemCollection, value);
        }

        private DelegateCommand<IList> _selectItemsCommand;

        public DelegateCommand<IList> SelectItemsCommand =>
            _selectItemsCommand ?? (_selectItemsCommand = new DelegateCommand<IList>(ExecuteSelectItemsCommand));

        private void ExecuteSelectItemsCommand(IList parameter)
        {
            var items = parameter.OfType<GroupItemModel>().ToArray();
            foreach (var item in items)
            {
                SelectedItemCollection.Add(item);
                UnselectedItemCollection.Remove(item);
            }
        }

        private DelegateCommand<IList> _unselectItemsCommand;

        public DelegateCommand<IList> UnselectItemsCommand =>
            _unselectItemsCommand ?? (_unselectItemsCommand = new DelegateCommand<IList>(ExecuteUnselectItemsCommand));

        private void ExecuteUnselectItemsCommand(IList parameter)
        {
            var items = parameter.OfType<GroupItemModel>().ToArray();
            foreach (var item in items)
            {
                SelectedItemCollection.Remove(item);
                UnselectedItemCollection.Add(item);
            }
        }

        private DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand).ObservesProperty(() => SelectedGroup));

        private async void ExecuteSaveCommand()
        {
            foreach (var item in SelectedItemCollection)
            {
                await _userDataRepository.AddWord(SelectedGroup.Id, item.Type, item.WordId);
            }
            FinishInteraction.Invoke();
        }

        private bool CanExecuteSaveCommand()
        {
            return SelectedGroup != null;
        }

        public AddToGroupViewModel(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }

        private AddToGroupNotification _notification;

        public INotification Notification
        {
            get => _notification;
            set
            {
                if (value is AddToGroupNotification notification)
                {
                    _notification = notification;
                    InitData(notification.Items);
                }
            }
        }

        private async void InitData(List<GroupItemModel> items)
        {
            GroupCollection.Clear();
            SelectedGroup = null;
            SelectedItemCollection.Clear();
            UnselectedItemCollection.Clear();
            GroupCollection.AddRange(await _userDataRepository.GetListGroup());
            if (GroupCollection.Count > 0)
            {
                SelectedGroup = GroupCollection[0];
            }

            UnselectedItemCollection.AddRange(items);
        }

        public Action FinishInteraction { get; set; }
    }
}