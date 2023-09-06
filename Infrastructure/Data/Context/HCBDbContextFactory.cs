using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data.Context
{
    public class HCBDbContextFactory : IDesignTimeDbContextFactory<HCBDbContext>
    {
        public HCBDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HCBDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HedaalCarBoots;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new HCBDbContext(optionsBuilder.Options);
        }
    }
}
