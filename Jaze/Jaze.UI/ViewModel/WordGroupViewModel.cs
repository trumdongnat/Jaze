using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jaze.UI.Definitions;
using Jaze.UI.Models;
using Jaze.UI.Repository;
using Jaze.UI.Services;
using Jaze.UI.Views;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Jaze.UI.ViewModel
{
    public class WordGroupViewModel : ViewModelBase
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;

        #region Properties

        private ObservableCollection<GroupModel> _groups = new ObservableCollection<GroupModel>();

        public ObservableCollection<GroupModel> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        private GroupModel _currentGroup;

        public GroupModel CurrentGroup
        {
            get => _currentGroup;
            set => SetProperty(ref _currentGroup, value, OnCurrentGroupChange);
        }

        private async void OnCurrentGroupChange()
        {
            if (CurrentGroup != null && (!CurrentGroup.IsLoadFull || CurrentGroup.Items.Any(item => !item.IsLoadFull)))
            {
                IsLoadingItems = true;
                await _userDataRepository.LoadFull(CurrentGroup);
                IsLoadingItems = false;
            }
            RaisePropertyChanged(nameof(Items));
        }

        public List<object> Items => CurrentGroup?.Items?.Select(item => item.Item).ToList();

        private bool _isLoadingItems;

        public bool IsLoadingItems
        {
            get => _isLoadingItems;
            set => SetProperty(ref _isLoadingItems, value);
        }

        #endregion Properties

        #region Commands

        private DelegateCommand<object> _showItemCommand;

        public DelegateCommand<object> ShowItemCommand =>
            _showItemCommand ?? (_showItemCommand = new DelegateCommand<object>(ExecuteShowItemCommand));

        private void ExecuteShowItemCommand(object item)
        {
            var parameter = new NavigationParameters
            {
                {ParamNames.Item, item }
            };
            _regionManager.RequestNavigate(RegionNames.GroupItemDisplay, nameof(ItemDisplayView), parameter);
        }

        private DelegateCommand _refreshGroupCollectionCommand;

        public DelegateCommand RefreshGroupCollectionCommand =>
            _refreshGroupCollectionCommand ?? (_refreshGroupCollectionCommand = new DelegateCommand(ExecuteRefreshGroupCollectionCommand));

        private void ExecuteRefreshGroupCollectionCommand()
        {
            InitGroups();
        }

        private DelegateCommand _addGroupCommand;

        public DelegateCommand AddGroupCommand =>
            _addGroupCommand ?? (_addGroupCommand = new DelegateCommand(ExecuteAddGroupCommand));

        private void ExecuteAddGroupCommand()
        {
            RaiseAddGroupRequest();
        }

        private DelegateCommand<GroupModel> _deleteGroupCommand;

        public DelegateCommand<GroupModel> DeleteGroupCommand =>
            _deleteGroupCommand ?? (_deleteGroupCommand = new DelegateCommand<GroupModel>(ExecuteDeleteGroupCommand));

        private async void ExecuteDeleteGroupCommand(GroupModel groupModel)
        {
            await _userDataRepository.DeleteGroup(groupModel.Id);
            Groups.Remove(groupModel);
        }

        private DelegateCommand<GroupModel> _editGroupCommand;

        public DelegateCommand<GroupModel> EditGroupCommand =>
            _editGroupCommand ?? (_editGroupCommand = new DelegateCommand<GroupModel>(ExecuteEditGroupCommand));

        private void ExecuteEditGroupCommand(GroupModel group)
        {
            RaiseEditGroupRequest(group);
        }

        #endregion Commands

        #region Interaction

        private void RaiseAddGroupRequest()
        {
            _dialogService.ShowAddGroupDialog(result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    Groups.Add(result.Parameters.GetValue<GroupModel>("Group"));
                }
            });
        }

        private void RaiseEditGroupRequest(GroupModel group)
        {
            _dialogService.ShowEditGroupDialog(group, result =>
            {
                //TODO
            });
        }

        #endregion Interaction

        public WordGroupViewModel(IRegionManager regionManager, IUserDataRepository userDataRepository, IDialogService dialogService)
        {
            _userDataRepository = userDataRepository;
            _regionManager = regionManager;
            _dialogService = dialogService;
            InitGroups();
        }

        private async void InitGroups()
        {
            Groups.Clear();
            Groups.AddRange(await _userDataRepository.GetListGroup());
        }
    }
}