using CallCenterService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CallCenterService.Configuration
{

    public class UserRoleSeed
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleSeed(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public async Task Seed()
        {
            if((await _roleManager.FindByNameAsync("Admin")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if ((await _roleManager.FindByNameAsync("Rejestrujący")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Rejestrujący" });
            }

            if ((await _roleManager.FindByNameAsync("Serwisant")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Serwisant" });
            }

            if ((await _roleManager.FindByNameAsync("Kierownik")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Kierownik" });
            }

            if ((await _roleManager.FindByNameAsync("Księgowa")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Księgowa" });
            }
        }
    }
}
