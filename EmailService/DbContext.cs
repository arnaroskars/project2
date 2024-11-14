using Microsoft.EntityFrameworkCore;
using EmailService.Models;

namespace EmailService.EmailDb
{
    public class EmailDbContext : DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) {} 

        public DbSet<Email> Emails { get; set;} = null!;
        
    }
}