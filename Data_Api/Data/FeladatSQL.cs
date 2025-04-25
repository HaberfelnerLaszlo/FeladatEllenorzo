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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tanulo>()
                .HasMany(e => e.Szorgalmik)
                .WithOne()
                .HasForeignKey("TanuloId")
                .IsRequired();
        }
    }
}