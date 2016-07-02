using Jaze.DAO.Model;

namespace Jaze.DAO
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DatabaseContext : DbContext
    {
        // Your context has been configured to use a 'Model' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Jaze.DAO.Model' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model' 
        // connection string in the application configuration file.
        public DatabaseContext()
            : base("name=ModelContainer")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
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
    }
}