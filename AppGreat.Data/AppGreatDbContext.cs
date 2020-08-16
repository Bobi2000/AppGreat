using AppGreat.Models;
using Microsoft.EntityFrameworkCore;

namespace AppGreat.Data
{
    public class AppGreatDbContext : DbContext
    {
        //public AppGreatDbContext() : base("AppGreatContext") { }

        public AppGreatDbContext() { }

        public AppGreatDbContext(DbContextOptions<AppGreatDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=AppGreat.db");
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }
    }
}
