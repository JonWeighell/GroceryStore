using FruitSystem.Data.Classes;
using Microsoft.EntityFrameworkCore;

namespace FruitSystem.Data
{
    public class GroceryStoreDbContext : DbContext
    {
        public GroceryStoreDbContext(DbContextOptions<GroceryStoreDbContext> options) : base(options)
        {            
        }

        public DbSet<Product> Products { get; set; }
    }
}