using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class HCBIdentityDbContext : IdentityDbContext<HCBUser>
    {
        public HCBIdentityDbContext(DbContextOptions<HCBIdentityDbContext> options)
            : base(options)
        {
        }
    }
}