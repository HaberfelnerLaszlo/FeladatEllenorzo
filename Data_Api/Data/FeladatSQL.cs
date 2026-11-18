using Microsoft.EntityFrameworkCore;

using System.Reflection.Metadata;

namespace Data_Api.Data
{
    public class FeladatSQL : DbContext
    {
        public FeladatSQL(DbContextOptions<FeladatSQL> options)
        : base(options) { }
        public DbSet<Szoveg> Szovegek => Set<Szoveg>();
        public DbSet<FeladatHiany> FeladatHianyok => Set<FeladatHiany>();
        public DbSet<HibasFeladat> HibasFeladatok => Set<HibasFeladat>();
        public DbSet<Szorgalmi> Szorgalmik => Set<Szorgalmi>();
        public DbSet<Tanulo> Tanulok => Set<Tanulo>();
        public DbSet<Pont> Pontok => Set<Pont>();
        public DbSet<PontLog> PontLogok => Set<PontLog>();
        public DbSet<Csoport> Csoportok => Set<Csoport>();
        public DbSet<Beiratkozas> Beiratkozasok => Set<Beiratkozas>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tanulo>()
                .HasMany(e => e.Szorgalmik)
                .WithOne()
                .HasForeignKey("TanuloId")
                .IsRequired();

            // A kliens DateTime-ot küld, de az adatbázisban a hiány dátuma csak nap (date oszlop).
            modelBuilder.Entity<FeladatHiany>()
                .Property(h => h.Datum)
                .HasColumnType("date");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            JeloldModositottTanulok();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            JeloldModositottTanulok();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Ha egy tanuló pontszáma változik, a LastModify is frissül, így a kliens szinkronja (tanulokids) észleli a változást.
        /// </summary>
        private void JeloldModositottTanulok()
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries<Tanulo>())
            {
                if (entry.State == EntityState.Modified && entry.Property(t => t.Pont).IsModified)
                {
                    entry.Entity.LastModify = DateTime.Now;
                }
            }
        }
    }
}