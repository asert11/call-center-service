using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CallCenterService.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {}

        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Servicer> Servicers { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Fault> Faults { get; set; }
        public DbSet<EventHistory> EventHistory { get; set; }
    }
}
