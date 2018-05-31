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
        private readonly DatabaseContext _dbContext;

        public UserSeed(DatabaseContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            if ((await _userManager.FindByNameAsync("admin")) == null)
            {
                var user = new ApplicationUser { UserName = "admin", Email = "admin@admin.com", FirstName = "Admin", LastName = "Admin",
                    Street = "Jagiellońska",
                    StreetNumber = "28",
                    PostCode = "30-006",
                    City = "Kraków"
                };
                var result = await _userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            if ((await _userManager.FindByNameAsync("Rejestrujacy")) == null)
            {
                var user = new ApplicationUser { UserName = "Rejestrujacy", Email = "Rejestrujacy@Rejestrujacy", FirstName = "Maciej", LastName = "Rejestrant",
                    Street = "Zielona",
                    StreetNumber = "28A",
                    PostCode = "33-036",
                    City = "Warszawa"
                };
                var result = await _userManager.CreateAsync(user, "Rejestrujący123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Rejestrujacy");
                    await _userManager.AddToRoleAsync(user, "Rejestrujący");
                }
            }
            if ((await _userManager.FindByNameAsync("AntoniNowak")) == null)
            {
                var user = new ApplicationUser { UserName = "AntoniNowak", Email = "AntoniNowak@AntoniNowak", FirstName = "Antoni", LastName = "Nowak",
                    Street = "Polechońska",
                    StreetNumber = "7",
                    ApartmentNumber = "11",
                    PostCode = "32-123",
                    City = "Sanok"
                };
                var result = await _userManager.CreateAsync(user, "AntoniNowak123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("AntoniNowak");
                    await _userManager.AddToRoleAsync(user, "Serwisant");

                    var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");
                    await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations {
                        ServicerId = user.Id,
                        Spec = spec
                    });
                    _dbContext.SaveChanges();
                }
            }
            if ((await _userManager.FindByNameAsync("AdamNowak")) == null)
            {
                var user = new ApplicationUser { UserName = "AdamNowak", Email = "AdamNowak@AdamNowak", /*Specialization = "RTV",*/ FirstName = "Adam", LastName = "Nowak",
                    Street = "29 Listopada",
                    StreetNumber = "31",
                    PostCode = "38-700",
                    City = "Ustrzyki Dolne"
                };
                var result = await _userManager.CreateAsync(user, "AdamNowak123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("AdamNowak");
                    await _userManager.AddToRoleAsync(user, "Serwisant");

                    var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");
                    await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations
                    {
                        ServicerId = user.Id,
                        Spec = spec
                    });
                    _dbContext.SaveChanges();
                }
            }
            if ((await _userManager.FindByNameAsync("ArturKopytko")) == null)
            {
                var user = new ApplicationUser { UserName = "ArturKopytko", Email = "ArturKopytko@ArturKopytko", /*Specialization = "RTV",*/ FirstName = "Artur", LastName = "Kopytko",
                    Street = "29 Listopada",
                    StreetNumber = "31",
                    PostCode = "38-700",
                    City = "Ustrzyki Dolne"
                };
                var result = await _userManager.CreateAsync(user, "ArturKopytko123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("ArturKopytko");
                    await _userManager.AddToRoleAsync(user, "Serwisant");

                    var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");
                    await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations
                    {
                        ServicerId = user.Id,
                        Spec = spec
                    });
                    _dbContext.SaveChanges();
                }
            }
            if ((await _userManager.FindByNameAsync("JanPrus")) == null)
            {
                var user = new ApplicationUser { UserName = "JanPrus", Email = "JanPrus@JanPrus", /*Specialization = "AGD",*/ FirstName = "Jan", LastName = "Prus",
                    Street = "Na Błonie",
                    StreetNumber = "11",
                    ApartmentNumber = "159",
                    PostCode = "30-147",
                    City = "Kraków"
                };
                var result = await _userManager.CreateAsync(user, "JanPrus123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("JanPrus");
                    await _userManager.AddToRoleAsync(user, "Serwisant");

                    var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");
                    await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations
                    {
                        ServicerId = user.Id,
                        Spec = spec
                    });
                    _dbContext.SaveChanges();
                }
            }
            if ((await _userManager.FindByNameAsync("KonradKania")) == null)
            {
                var user = new ApplicationUser { UserName = "KonradKania", Email = "KonradKania@KonradKania", /*Specialization = "AGD",*/ FirstName = "Konrad", LastName = "Kania",
                    Street = "Korczaka",
                    StreetNumber = "1B",
                    PostCode = "32-221",
                    City = "Lesko"
                };
                var result = await _userManager.CreateAsync(user, "KonradKania123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("KonradKania");
                    await _userManager.AddToRoleAsync(user, "Serwisant");

                    var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");
                    await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations
                    {
                        ServicerId = user.Id,
                        Spec = spec
                    });
                    _dbContext.SaveChanges();
                }
            }
            if ((await _userManager.FindByNameAsync("KamilBem")) == null)
            {
                var user = new ApplicationUser { UserName = "KamilBem", Email = "KamilBem@KamilBem", /*Specialization = "AGD",*/ FirstName = "Kamil", LastName = "Bem",
                    Street = "Długa",
                    StreetNumber = "8",
                    ApartmentNumber = "12",
                    PostCode = "39-195",
                    City = "Rzeszów"
                };
                var result = await _userManager.CreateAsync(user, "KamilBem123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("KamilBem");
                    await _userManager.AddToRoleAsync(user, "Serwisant");

                    var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");
                    await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations
                    {
                        ServicerId = user.Id,
                        Spec = spec
                    });
                    _dbContext.SaveChanges();
                }
            }
            if ((await _userManager.FindByNameAsync("Kierownik")) == null)
            {
                var user = new ApplicationUser { UserName = "Kierownik", Email = "Kierownik@Kierownik", FirstName = "Robert", LastName = "Kierowniczy",
                    Street = "Rzeczna",
                    StreetNumber = "5",
                    PostCode = "33-111",
                    City = "Rabe"
                };
                var result = await _userManager.CreateAsync(user, "Kierownik123!");
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync("Kierownik");
                    await _userManager.AddToRoleAsync(user, "Kierownik");
                }
            }
            if ((await _userManager.FindByNameAsync("Ksiegowa")) == null)
            {
                var user = new ApplicationUser { UserName = "Ksiegowa", Email = "Ksiegowa@Ksiegowa", FirstName = "Marzena", LastName = "Księga",
                    Street = "Na Błonie",
                    StreetNumber = "11",
                    ApartmentNumber = "159",
                    PostCode = "30-147",
                    City = "Kraków"
                };
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
