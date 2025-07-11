using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using gymWebsite.Models;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Server=localhost;Port=3306;Database=gymDB;UserId=root;Password=;AllowPublicKeyRetrieval=true;";
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
        }
    }
}