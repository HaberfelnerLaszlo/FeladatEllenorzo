using Microsoft.EntityFrameworkCore;

namespace Data_Api.Data
{
	public class FeladatDb:DbContext
	{
        public FeladatDb(DbContextOptions<FeladatDb> options)
        : base(options) { }
        public DbSet<Szoveg>Szovegek =>Set<Szoveg>();
        public DbSet<FeladatHiany> FeladatHianyok =>Set<FeladatHiany>();
        public DbSet<HibasFeladat> HibasFeladatok => Set<HibasFeladat>();
        public DbSet<Szorgalmi> Szorgalmik =>Set<Szorgalmi>();
        public DbSet<Tanulo> Tanulok => Set<Tanulo>();
        public DbSet<Pont> Pontok => Set<Pont>();

    }
}
