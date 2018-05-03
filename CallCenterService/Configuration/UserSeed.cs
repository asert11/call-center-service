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
            if ((await _userManager.FindByNameAsync("admin")) == null)
            {
                var user = new ApplicationUser { UserName = "admin", Email = "admin@admin.com" };
                var result = await _userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            if ((await _userManager.FindByNameAsync("Rejestrujacy")) == null)
            {
                var user = new ApplicationUser { UserName = "Rejestrujacy", Email = "Rejestrujacy@Rejestrujacy" };
                var result = await _userManager.CreateAsync(user, "Rejestrujący123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Rejestrujacy");
                    await _userManager.AddToRoleAsync(user, "Rejestrujący");
                }
            }
            if ((await _userManager.FindByNameAsync("Serwisant")) == null)
            {
                var user = new ApplicationUser { UserName = "Serwisant", Email = "Serwisant@Serwisant" };
                var result = await _userManager.CreateAsync(user, "Serwisant123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Serwisant");
                    await _userManager.AddToRoleAsync(user, "Serwisant");
                }
            }
            if ((await _userManager.FindByNameAsync("Kierownik")) == null)
            {
                var user = new ApplicationUser { UserName = "Kierownik", Email = "Kierownik@Kierownik" };
                var result = await _userManager.CreateAsync(user, "Kierownik123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Kierownik");
                    await _userManager.AddToRoleAsync(user, "Kierownik");
                }
            }
            if ((await _userManager.FindByNameAsync("Ksiegowa")) == null)
            {
                var user = new ApplicationUser { UserName = "Ksiegowa", Email = "Ksiegowa@Ksiegowa" };
                var result = await _userManager.CreateAsync(user, "Ksiegowa123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Ksiegowa");
                    await _userManager.AddToRoleAsync(user, "Księgowa");
                }
            }
        }
    }
}
