using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CallCenterService.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {}

        public DbSet<Fault> Faults { get; set; }
        public DbSet<ServicerFault> ServicerFault { get; set; }
        
    }
}
