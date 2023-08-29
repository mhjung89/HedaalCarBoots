using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class HCBDbContext : DbContext
    {
        public HCBDbContext(DbContextOptions<HCBDbContext> options)
            : base(options)
        {
        }

        public DbSet<TradeItem> TradeItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<TradeItem>()
                    .ToTable("TradeItem")
                    .Property(p => p.Price)
                        .HasColumnType("decimal(18,2)");
        }
    }
}
