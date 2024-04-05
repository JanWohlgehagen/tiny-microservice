using Microsoft.EntityFrameworkCore;
using User.Models;

namespace User.Data.repo
{
    public class Repo : IRepo
    {
        public Context Context { get; set; }
        public Repo(Context context) 
        {
            Context = context;
        }
        public DbSet<Models.User> Users()
        {
            return Context.Users;
        }

        public DbSet<UserSettings> UserSettings()
        {
            return Context.UserSettings;
        }

        public async Task SaveChangesAsync()
        {
             await  Context.SaveChangesAsync();
        }
    }
}
