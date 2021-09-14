using Microsoft.EntityFrameworkCore;
using ShopApp.Entities.Concrete;
using ShopApp.Core.Entities.Concrete;

namespace ShopApp.DataAccess.Concrete.EntityFramework.Contexts
{
    public class ShopAppContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-OF69QGO;Database=ShopAppDB;Trusted_Connection=true");
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
