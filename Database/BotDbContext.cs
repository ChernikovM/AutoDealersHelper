using AutoDealersHelper.Database.Objects;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AutoDealersHelper.Database
{
    public class BotDbContext : DbContext
    {
        public static int BrandsCount { get; private set; }
        public static int FuelsCount { get; private set; }
        public static int GearBoxesCount { get; private set; }
        public static int StatesCount { get; private set; }

        private readonly string _dbPath;
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<GearBox> GearBoxes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<User> Users { get; set; }

        public BotDbContext() : base()
        {
            _dbPath = $"{Directory.GetCurrentDirectory()}/BotDb.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source={_dbPath};Cache=Shared");
        }

    }
}
