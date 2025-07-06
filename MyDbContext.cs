// MyDbContext.cs
using Microsoft.EntityFrameworkCore;
using gymWebsite; // This references your User model

namespace gymWebsite.Data // Recommended namespace
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // This creates the Users table
    }
}