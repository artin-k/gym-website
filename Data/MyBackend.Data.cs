using Microsoft.EntityFrameworkCore;

namespace MyBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        { }

        public DbSet<User> users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;  // Default value
        public string Password { get; set; } = string.Empty;  // Default value
    }

}



