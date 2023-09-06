using Core.Authentication;
using Core.Authorization;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class HCBDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public HCBDbContext(DbContextOptions<HCBDbContext> options)
            : base(options)
        {
        }

        public DbSet<TradeItem> TradeItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TradeItem>()
                .ToTable("TradeItem");
            modelBuilder.Entity<TradeItem>()
                .Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<TradeItem>()
                .Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<TradeItem>()
                .Property(p => p.CreatedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TradeItem>()
                .Property(p => p.UpdatedAt).HasDefaultValueSql("getdate()");
        }
    }
}
