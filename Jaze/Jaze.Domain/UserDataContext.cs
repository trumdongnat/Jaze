using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Jaze.Domain.Definitions;
using Jaze.Domain.Entities;

namespace Jaze.Domain
{
    public class UserDataContext : DbContext
    {
        public UserDataContext()
            : base("name=UserDataContext")
        {
            Database.SetInitializer(new UserDataInitializer());
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupItem> GroupItems { get; set; }
        public DbSet<History> Histories { get; set; }
    }

    internal class UserDataInitializer : CreateDatabaseIfNotExists<UserDataContext>
    {
        protected override void Seed(UserDataContext context)
        {
            base.Seed(context);
            if (!context.Groups.Any())
            {
                using (var db = new JazeDatabaseContext())
                {
                    var kanjis = db.Kanjis.ToArray();
                    var levels = new[] { Level.N1, Level.N2, Level.N3, Level.N4, Level.N5 };
                    foreach (var level in levels)
                    {
                        var groupItems = kanjis.Where(kanji => kanji.Level == level).Select(kanji => new GroupItem { Type = DictionaryType.Kanji, WordId = kanji.Id });
                        var group = new Group
                        {
                            Name = "Kanji " + level,
                            Items = groupItems.ToList()
                        };
                        context.Groups.Add(group);
                    }
                }

                context.SaveChanges();
            }
        }
    }
}