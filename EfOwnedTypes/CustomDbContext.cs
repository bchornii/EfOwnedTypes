using EfOwnedTypes.Models;
using EfOwnedTypes.Models.Customers;
using Microsoft.EntityFrameworkCore;

namespace EfOwnedTypes
{
    public class CustomDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomDbContext(DbContextOptions<CustomDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomDbContext).Assembly);
        }
    }
}
