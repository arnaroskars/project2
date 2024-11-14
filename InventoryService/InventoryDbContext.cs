using Microsoft.EntityFrameworkCore;
using InventoryService.Models;

namespace InventoryService.InventoryDb
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) {} 

        public DbSet<Product> Products { get; set;} = null!;
        
    }
}