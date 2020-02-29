using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;
using Jaze.UI.Models;

namespace Jaze.UI.Services.UserData
{
    public interface IGroupService
    {
        int AddGroup(string name);

        void ChangeGroupName(int id, string name);

        GroupModel GetGroup(int id);

        List<GroupModel> GetListGroup();

        void DeleteGroup(int id);

        void AddWord(int groupId, DictionaryType type, int wordId);

        void RemoveWord(int groupId, DictionaryType type, int wordId);

        void LoadFull(GroupModel group);
    }
}