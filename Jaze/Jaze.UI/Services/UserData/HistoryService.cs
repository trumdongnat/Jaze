using System;
using System.Linq;
using Jaze.Domain;
using Jaze.Domain.Definitions;
using Jaze.Domain.Entities;

namespace Jaze.UI.Services.UserData
{
    public class HistoryService : IHistoryService
    {
        public void Add(DictionaryType type, int id)
        {
            using (var db = new UserDataContext())
            {
                var history = db.Histories.FirstOrDefault(t => t.Type == type && t.Id == id);
                if (history != null)
                {
                    history.LastTime = DateTime.Now;
                }
                else
                {
                    db.Histories.Add(new History() { Id = id, Type = type, LastTime = DateTime.Now });
                }
                db.SaveChanges();
            }
        }

        public void Add(DictionaryType type, int id, DateTime time)
        {
            using (var db = new UserDataContext())
            {
                var history = db.Histories.FirstOrDefault(t => t.Type == type && t.Id == id);
                if (history != null)
                {
                    history.LastTime = time;
                }
                else
                {
                    db.Histories.Add(new History() { Id = id, Type = type, LastTime = time });
                }
                db.SaveChanges();
            }
        }

        public void Remove(DictionaryType type, int id)
        {
            using (var db = new UserDataContext())
            {
                var history = db.Histories.FirstOrDefault(t => t.Type == type && t.Id == id);
                if (history != null)
                {
                    db.Histories.Remove(history);
                    db.SaveChanges();
                }
            }
        }
    }
}