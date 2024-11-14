using Microsoft.EntityFrameworkCore;
using MerchantService.Models;

namespace MerchantService.OrderDb
{
    public class MerchantDbContext : DbContext
    {
        public MerchantDbContext(DbContextOptions<MerchantDbContext> options) : base(options) {} 

        public DbSet<Merchant> Merchants { get; set;} = null!;
        
    }
}