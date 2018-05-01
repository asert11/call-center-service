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
<<<<<<< HEAD
        public DbSet<EventHistory> EventHistory { get; set; }
=======
    
        public DbSet<ServicerFault> ServicerFault { get; set; }
        
>>>>>>> 1013575470d6e80506e49ced4d6d5ff949062d51
    }
}
