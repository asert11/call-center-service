using CallCenterService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Configuration
{
    public class Seed
    {
        private readonly DatabaseContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public Seed(DatabaseContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void SeedDatabase()
        {
            await new UserRoleSeed(_roleManager).Seed();
            await new UserSeed(_userManager).Seed();
            await new ClientsSeed(_dbContext).Seed();
            await new ProductsSeed(_dbContext).Seed();
            await new FaultsSeed(_dbContext).Seed();
            await new RepairsSeed(_dbContext,_userManager).Seed();

        }
    }
}
