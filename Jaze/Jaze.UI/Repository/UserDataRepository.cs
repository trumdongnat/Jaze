using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;
using Jaze.UI.Models;
using Jaze.UI.Services.UserData;

namespace Jaze.UI.Repository
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly IGroupService _groupService;
        private readonly IHistoryService _historyService;
        private readonly IDictionaryRepository _dictionaryRepository;

        public UserDataRepository(IGroupService groupService, IHistoryService historyService, IDictionaryRepository dictionaryRepository)
        {
            _groupService = groupService;
            _historyService = historyService;
            _dictionaryRepository = dictionaryRepository;
        }

        public Task<int> AddGroup(string name)
        {
            return Task.Run(() => _groupService.AddGroup(name));
        }

        public Task ChangeGroupName(int id, string name)
        {
            return Task.Run(() => _groupService.ChangeGroupName(id, name));
        }

        public Task<GroupModel> GetGroup(int id)
        {
            return Task.Run(() =>
            {
                var group = _groupService.GetGroup(id);
                _groupService.LoadFull(group);
                return group;
            });
        }

        public Task<List<GroupModel>> GetListGroup()
        {
            return Task.Run(() =>
            {
                var groups = _groupService.GetListGroup();
                foreach (var @group in groups)
                {
                    _groupService.LoadFull(@group);
                }
                return groups;
            });
        }

        public Task DeleteGroup(int id)
        {
            return Task.Run(() => _groupService.DeleteGroup(id));
        }

        public Task AddWord(int groupId, DictionaryType type, int wordId)
        {
            return Task.Run(() => _groupService.AddWord(groupId, type, wordId));
        }

        public Task RemoveWord(int groupId, DictionaryType type, int wordId)
        {
            return Task.Run(() => _groupService.RemoveWord(groupId, type, wordId));
        }

        public Task LoadFull(GroupModel group)
        {
            return Task.Run(async () =>
            {
                _groupService.LoadFull(group);
                foreach (var item in group.Items)
                {
                    await LoadFull(item);
                }
            });
        }

        public async Task LoadFull(GroupItemModel item)
        {
            item.Item = await _dictionaryRepository.GetAsync(item.Type, item.WordId);
            item.IsLoadFull = true;
        }

        public Task AddHistory(DictionaryType type, int id)
        {
            return Task.Run(() => _historyService.Add(type, id));
        }

        public Task AddHistory(DictionaryType type, int id, DateTime time)
        {
            return Task.Run(() => _historyService.Add(type, id, time));
        }

        public Task RemoveHistory(DictionaryType type, int id)
        {
            return Task.Run(() => _historyService.Remove(type, id));
        }
    }
}