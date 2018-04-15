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
            if ((await _userManager.FindByNameAsync("Rejestrujacy@Rejestrujacy")) == null)
            {
                var user = new ApplicationUser { UserName = "Rejestrujacy@Rejestrujacy", Email = "Rejestrujacy@Rejestrujacy" };
                var result = await _userManager.CreateAsync(user, "Rejestrujący123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Rejestrujacy@Rejestrujacy");
                    await _userManager.AddToRoleAsync(user, "Rejestrujący");
                }
            }
            if ((await _userManager.FindByNameAsync("Serwisant@Serwisant")) == null)
            {
                var user = new ApplicationUser { UserName = "Serwisant@Serwisant", Email = "Serwisant@Serwisant" };
                var result = await _userManager.CreateAsync(user, "Serwisant123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Serwisant@Serwisant");
                    await _userManager.AddToRoleAsync(user, "Serwisant");
                }
            }
            if ((await _userManager.FindByNameAsync("Kierownik@Kierownik")) == null)
            {
                var user = new ApplicationUser { UserName = "Kierownik@Kierownik", Email = "Kierownik@Kierownik" };
                var result = await _userManager.CreateAsync(user, "Kierownik123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Kierownik@Kierownik");
                    await _userManager.AddToRoleAsync(user, "Kierownik");
                }
            }
            if ((await _userManager.FindByNameAsync("Ksiegowa@Ksiegowa")) == null)
            {
                var user = new ApplicationUser { UserName = "Ksiegowa@Ksiegowa", Email = "Ksiegowa@Ksiegowa" };
                var result = await _userManager.CreateAsync(user, "Ksiegowa123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Ksiegowa@Ksiegowa");
                    await _userManager.AddToRoleAsync(user, "Księgowa");
                }
            }
        }
    }
}
