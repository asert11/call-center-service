using CallCenterService.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Configuration
{
    public class UserSeed
    {
        private UserManager<ApplicationUser> _userManager;

        public UserSeed(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Seed()
        {
            if ((await _userManager.FindByNameAsync("admin@admin.com")) == null)
            {
                var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                var result = await _userManager.CreateAsync(user, "Admin123!");
                if(result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("admin@admin.com");
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
