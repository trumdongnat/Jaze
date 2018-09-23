using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jaze.Domain;
using Jaze.Domain.Definitions;
using Jaze.Domain.Entities;
using Jaze.UI.Models;

namespace Jaze.UI.Services.UserData
{
    public class GroupService : IGroupService
    {
        public int AddGroup(string name)
        {
            using (var db = new UserDataContext())
            {
                var group = db.Groups.FirstOrDefault(t => t.Name == name);
                if (group != null)
                {
                    return group.Id;
                }
                else
                {
                    var newGroup = new Group { Name = name };
                    db.Groups.Add(newGroup);
                    db.SaveChanges();
                    return newGroup.Id;
                }
            }
        }

        public void ChangeGroupName(int id, string name)
        {
            using (var db = new UserDataContext())
            {
                var group = db.Groups.Find(id);
                if (group != null)
                {
                    group.Name = name;
                    db.SaveChanges();
                }
            }
        }

        public GroupModel GetGroup(int id)
        {
            using (var db = new UserDataContext())
            {
                var group = db.Groups.Find(id);
                if (group != null)
                {
                    return new GroupModel(group);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<GroupModel> GetListGroup()
        {
            using (var db = new UserDataContext())
            {
                return db.Groups.ToList().Select(group => new GroupModel(group)).ToList();
            }
        }

        public void DeleteGroup(int id)
        {
            using (var db = new UserDataContext())
            {
                var group = new Group() { Id = id };
                db.Groups.Attach(group);
                db.Groups.Remove(group);
                db.SaveChanges();
            }
        }

        public void AddWord(int groupId, DictionaryType type, int wordId)
        {
            using (var db = new UserDataContext())
            {
                var group = db.Groups.Find(groupId);
                if (group == null)
                {
                    return;
                }

                var item = group.Items.FirstOrDefault(t => t.Type == type && t.WordId == wordId);
                if (item != null)
                {
                    return;
                }
                group.Items.Add(new GroupItem()
                {
                    Type = type,
                    WordId = wordId
                });
                db.SaveChanges();
            }
        }

        public void RemoveWord(int groupId, DictionaryType type, int wordId)
        {
            using (var db = new UserDataContext())
            {
                var group = db.Groups.Find(groupId);
                if (group == null)
                {
                    return;
                }

                var item = group.Items.FirstOrDefault(t => t.Type == type && t.WordId == wordId);
                if (item != null)
                {
                    db.GroupItems.Remove(item);
                    db.SaveChanges();
                }
            }
        }

        public void LoadFull(GroupModel group)
        {
            using (var db = new UserDataContext())
            {
                var entity = db.Groups.Find(group.Id);
                if (entity == null)
                {
                    throw new ArgumentException();
                }

                group.Items = new ObservableCollection<GroupItemModel>();
                foreach (var item in entity.Items)
                {
                    group.Items.Add(new GroupItemModel(item));
                }

                group.IsLoadFull = true;
            }
        }
    }
}