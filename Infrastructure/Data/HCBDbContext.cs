using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class HCBDbContext : DbContext
    {
        public HCBDbContext(DbContextOptions<HCBDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
