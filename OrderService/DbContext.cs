using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.OrderDb
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) {} 

        public DbSet<Order> Orders { get; set;} = null!;
        
    }
}