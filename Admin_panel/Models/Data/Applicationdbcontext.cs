using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Admin_panel.Models.Data
{
    public class Applicationdbcontext:IdentityDbContext
    {
        public Applicationdbcontext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SuperMarket> SuperMarkets { get; set; }
        public DbSet<Product>Products { get; set; }
        public DbSet<ApplicationUser>ApplicationUsers { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
