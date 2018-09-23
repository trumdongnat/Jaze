using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;
using Jaze.UI.Models;

namespace Jaze.UI.Repository
{
    public interface IUserDataRepository
    {
        Task<int> AddGroup(string name);

        Task ChangeGroupName(int id, string name);

        Task<GroupModel> GetGroup(int id);

        Task<List<GroupModel>> GetListGroup();

        Task DeleteGroup(int id);

        Task AddWord(int groupId, DictionaryType type, int wordId);

        Task RemoveWord(int groupId, DictionaryType type, int wordId);

        Task LoadFull(GroupModel group);

        Task LoadFull(GroupItemModel item);

        Task AddHistory(DictionaryType type, int id);

        Task AddHistory(DictionaryType type, int id, DateTime time);

        Task RemoveHistory(DictionaryType type, int id);
    }
}