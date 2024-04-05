using Microsoft.EntityFrameworkCore;

namespace User.Data.repo
{
    public interface IRepo
    {
        public DbSet<Models.User> Users();
        public DbSet<Models.UserSettings> UserSettings();

        public Task SaveChangesAsync();
    }
}
