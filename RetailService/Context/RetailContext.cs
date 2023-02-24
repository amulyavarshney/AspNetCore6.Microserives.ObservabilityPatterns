using Microsoft.EntityFrameworkCore;
using RetailService.Models;

namespace RetailService.Context
{
    public class RetailContext : DbContext
    {
        public RetailContext(DbContextOptions<RetailContext> options) : base(options) { }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable(nameof(Item));
        }
    }
}
