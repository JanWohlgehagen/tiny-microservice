using Microsoft.EntityFrameworkCore;

namespace User.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {
        }

        // Specify a default location for the SQLite database
        private const string DefaultDbPath = "WannaBeFirmaSQLite.db";

        // Define a DbSet for your Result entity
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.UserSettings> UserSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Always use the default location for the SQLite database
            options.UseSqlite($"Data Source={DefaultDbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>().HasKey(r => r.id);
            modelBuilder.Entity<Models.UserSettings>().HasKey(r => r.id);
        }

    }
}
