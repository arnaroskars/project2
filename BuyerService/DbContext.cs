using Microsoft.EntityFrameworkCore;
using BuyerService.Models;

namespace BuyerService.OrderDb
{
    public class BuyerDbContext : DbContext
    {
        public BuyerDbContext(DbContextOptions<BuyerDbContext> options) : base(options) {} 

        public DbSet<Buyer> Buyers { get; set;} = null!;
        
    }
}