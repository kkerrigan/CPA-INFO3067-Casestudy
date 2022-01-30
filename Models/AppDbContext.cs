using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Casestudy.Models
{

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Brand>().ForSqlServerToTable("Brands")
                .Property(c => c.Id).UseSqlServerIdentityColumn();
            builder.Entity<Product>().ForSqlServerToTable("Products");
            builder.Entity<Order>().ForSqlServerToTable("Orders")
                .Property(o => o.Id).UseSqlServerIdentityColumn();
            builder.Entity<OrderLineItem>().ForSqlServerToTable("OrderLineItems")
                .Property(l => l.Id).UseSqlServerIdentityColumn();
            builder.Entity<Branch>().ForSqlServerToTable("Branches")
                .Property(b => b.Id).UseSqlServerIdentityColumn();
        }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderLineItem> OrderLineItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
    }
}
