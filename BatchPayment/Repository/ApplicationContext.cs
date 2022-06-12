using Entities;
using Microsoft.EntityFrameworkCore;

namespace BatchPayment.Repository
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();
    }
}
