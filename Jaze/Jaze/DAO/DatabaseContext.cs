using Jaze.Model;

namespace Jaze.DAO
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DatabaseContext : DbContext
    {
        
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public DbSet<JaEnExample> JaEnExamples { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<HanViet> HanViets { get; set; }
        public DbSet<JaVi> JaVis { get; set; }
        public DbSet<JaEn> JaEns { get; set; }
        public DbSet<Kanji> Kanjis { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Radical> Radicals { get; set; }
        public DbSet<JaViExample> JaViExamples { get; set; }
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<ViJa> ViJas { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kanji>().
                HasMany(k => k.Parts).
                WithMany(p => p.Kanjis).
                Map(m =>
                {
                    m.MapLeftKey("KanjiId ");
                    m.MapRightKey("PartId ");
                    m.ToTable("Kanji_Part");
                });

        }

        /********************************************************************/
        /********************************************************************/
        /********************************************************************/

        public static DatabaseContext Context { get; private set; }

        static DatabaseContext()
        {
            Context = new DatabaseContext();
        }

        public static void Refresh()
        {
            Context.Dispose();
            Context = new DatabaseContext();
        }
    }
}