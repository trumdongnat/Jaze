using System.Data.Entity;
using Jaze.Domain.Entities;
using Jaze.Domain.Migrations;

namespace Jaze.Domain
{
    public class JazeDatabaseContext : DbContext
    {
        public JazeDatabaseContext()
            : base("name=JazeDatabaseContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<JazeDatabaseContext, Configuration>("JazeDatabaseContext"));
        }

        public DbSet<JaEnExample> JaEnExamples { get; set; }
        public DbSet<HanViet> HanViets { get; set; }
        public DbSet<JaVi> JaVis { get; set; }
        public DbSet<JaEn> JaEns { get; set; }
        public DbSet<Kanji> Kanjis { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Radical> Radicals { get; set; }
        public DbSet<JaViExample> JaViExamples { get; set; }
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<ViJa> Vijas { get; set; }

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
    }
}