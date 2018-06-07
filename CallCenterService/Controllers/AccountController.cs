using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenterService.ViewModels;
using CallCenterService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CallCenterService.Controllers
{
    [Authorize(Roles = "Admin , Serwisant , Kierownik , Ksiêgowa , Rejestruj¹cy")]

    public class AccountController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(DatabaseContext dbContext, UserManager<ApplicationUser> userManager, 
                                 SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.Title = "Login Page";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "Niepoprawny login lub has³o");
                return View(vm);
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            var vm = new AddUserViewModel
            {
                Roles = GetUserRoles()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Role == "Admin")
                {
                    vm.Roles = GetUserRoles();
                    return View(vm);
                }

                ApplicationUser loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                string id = loggedUser?.Id;
                if (id == null)
                    return NotFound();

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    var user = new ApplicationUser { UserName = vm.UserName, Email = vm.Email,
                        FirstName = vm.FirstName, LastName = vm.LastName, Street = vm.Street,
                        StreetNumber = vm.StreetNumber, ApartmentNumber = vm.ApartmentNumber,
                        PostCode = vm.PostCode, City = vm.City };

                    var result = await _userManager.CreateAsync(user, vm.Password);
                    _dbContext.SaveChanges();

                    if (!result.Succeeded)
                    {
                        transaction.Rollback();
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        vm.Roles = GetUserRoles();
                        return View(vm);
                    }

                    user = await _userManager.FindByNameAsync(vm.UserName);
                    await _userManager.AddToRoleAsync(user, vm.Role);

                    var history = new EventHistory {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "add user",
                        Table = "AspNetUsers",
                        Description = "UserId{" + user.Id + "} " +
                            "UserName{" + vm.UserName + "} " +
                            "Email{" + vm.Email + "} " +
                            "FirstName{" + vm.FirstName + "} " +
                            "LastName{" + vm.LastName + "} " +
                            "Street{" + vm.Street + "} " +
                            "StreetNumber{" + vm.StreetNumber + "} " +
                            "ApartmentNumber{" + vm.ApartmentNumber + "} " +
                            "PostCode{" + vm.PostCode + "} " +
                            "City{" + vm.City + "}"
                    };
                   

                    _dbContext.EventHistory.Add(history);

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "add user role",
                        Table = "AspNetUserRoles",
                        Description = "UserId{" + user.Id + "} " +
                                      "Role{" + vm.Role + "}"
                    };

                    _dbContext.EventHistory.Add(history);

                    _dbContext.SaveChanges();

                    transaction.Commit();
                    
                    return RedirectToAction("Index");
                }
            }
            vm.Roles = GetUserRoles();
            return View(vm);
        }

        public async Task <IActionResult> Index(string searchUserLogin, string searchUserEmail, string searchUserRole)
        {

            //var name = (from m in _dbContext.Users
            //           select m);

            //var role  = from n in _userManager.Users
            //            select n;


            IList<ApplicationUser> name;

            if (!String.IsNullOrEmpty(searchUserRole))
            {
                name = await _userManager.GetUsersInRoleAsync(searchUserRole);
            }
            else
                name = _userManager.Users.ToList();

            if (!String.IsNullOrEmpty(searchUserLogin))
            {
                name = name.Where(s=>s.UserName.Equals(searchUserLogin)).ToList();
            }

            if (!String.IsNullOrEmpty(searchUserEmail))
            {
                name = name.Where(s => s.Email.Equals(searchUserEmail)).ToList();
            }

            var vm = new UsersViewModel()
            {
                Users = name.ToList()
            };
            return View(vm);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await GetUserById(id);
            if (user == null)
                return RedirectToAction("Index");

            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                EventHistory history;

                var servicerSpecializations = _dbContext.ServicerSpecializations.Include(m => m.Spec)
                    .Where(m => m.ServicerId == id);

                foreach(var specialization in servicerSpecializations)
                {
                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete servicer specialization",
                        Table = "ServicerSpecializations",
                        Description = "Id{" + specialization.Id + "} " +
                                      "ServicerId{" + specialization.ServicerId + "} " +
                                      "SpecId{" + specialization.Spec.Id + "}"
                    };
                    _dbContext.EventHistory.Add(history);
                    _dbContext.ServicerSpecializations.Remove(specialization);
                }
                _dbContext.SaveChanges();

                //_dbContext.ServicerSpecializations.RemoveRange(servicerSpecializations);

                var repairs = _dbContext.Repairs.Include(m => m.Fault).Include(m => m.Fault.Product)
                    .Where(m => m.Fault.Status == "Done").Where(m => m.ServicerId == id).ToList();

                foreach(var repair in repairs)
                {
                    var fault = repair.Fault;

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete repair",
                        Table = "Repairs",
                        Description = "Date{" + repair.Date + "} " +
                                      "Description{" + repair.Description + "} " +
                                      "FaultId{" + repair.Fault.FaultId + "} " +
                                      "PartsPrice{" + repair.PartsPrice + "} " +
                                      "Price{" + repair.Price + "} " +
                                      "RepairId{" + repair.RepairId + "} " +
                                      "ServicerId{" + repair.ServicerId + "}"
                    };
                    _dbContext.EventHistory.Add(history);

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete fault",
                        Table = "Faults",
                        Description = "ApplicationDate{" + fault.ApplicationDate + "} " +
                                      "ClientDescription{" + fault.ClientDescription + "} " +
                                      "FaultId{" + fault.FaultId + "} " +
                                      "Status{" + fault.Status + "} " +
                                      "ProductID{" + fault.Product.ProductID + "}"
                    };
                    _dbContext.EventHistory.Add(history);

                    _dbContext.Repairs.Remove(repair);
                    _dbContext.Faults.Remove(fault);
                }
                _dbContext.SaveChanges();

                repairs = _dbContext.Repairs.Include(m => m.Fault).Include(m => m.Fault.Product)
                    .Where(m => m.Fault.Status != "Done").Where(m => m.ServicerId == id).ToList();

                foreach(var repair in repairs) {
                    repair.Fault.Status = "Open";

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "edit fault",
                        Table = "Faults",
                        Description = "ApplicationDate{" + repair.Fault.ApplicationDate + "} " +
                                      "ClientDescription{" + repair.Fault.ClientDescription + "} " +
                                      "FaultId{" + repair.Fault.FaultId + "} " +
                                      "Status{" + repair.Fault.Status + "} " +
                                      "ProductID{" + repair.Fault.Product.ProductID + "}"
                    };
                    _dbContext.EventHistory.Add(history);

                    _dbContext.Faults.Update(repair.Fault);

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete repair",
                        Table = "Repairs",
                        Description = "Date{" + repair.Date + "} " +
                                      "Description{" + repair.Description + "} " +
                                      "FaultId{" + repair.Fault.FaultId + "} " +
                                      "PartsPrice{" + repair.PartsPrice + "} " +
                                      "Price{" + repair.Price + "} " +
                                      "RepairId{" + repair.RepairId + "} " +
                                      "ServicerId{" + repair.ServicerId + "}"
                    };
                    _dbContext.EventHistory.Add(history);

                    _dbContext.Repairs.Remove(repair);
                }
                _dbContext.SaveChanges();

                //_dbContext.Repairs.RemoveRange(repairs);

                await _dbContext.SaveChangesAsync();

                var logins = user.Logins;
                var rolesForUser = await _userManager.GetRolesAsync(user);
                
                foreach (var item in rolesForUser.ToList())
                {
                    if (item == "Admin")
                        return RedirectToAction("Index");
                }

                foreach (var login in logins.ToList())
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "delete user role",
                            Table = "AspNetUserRoles",
                            Description = "UserId{" + user.Id + "} " +
                                      "Role{" + item + "}"
                        };

                        _dbContext.EventHistory.Add(history);

                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }
                _dbContext.SaveChanges();

                history = new EventHistory
                {
                    Date = DateTime.Now,
                    UserId = loggedUser.Id,
                    Operation = "delete user",
                    Table = "AspNetUsers",
                    Description = "UserId{" + user.Id + "} " +
                                      "UserName{" + user.UserName + "} " +
                                      "Email{" + user.Email + "} " +
                                      "FirstName{" + user.FirstName + "} " +
                                      "LastName{" + user.LastName + "} " +
                                      "Street{" + user.Street + "} " +
                                      "StreetNumber{" + user.StreetNumber + "} " +
                                      "ApartmentNumber{" + user.ApartmentNumber + "} " +
                                      "PostCode{" + user.PostCode + "} " +
                                      "City{" + user.City + "}"
                };
                _dbContext.EventHistory.Add(history);
                _dbContext.SaveChanges();

                await _userManager.DeleteAsync(user);
                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await GetUserById(id);
            if (user == null)
                return RedirectToAction("Index");

            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach (var item in rolesForUser.ToList())
            {
                if (item == "Admin")
                    return RedirectToAction("Index");
            }

            WorkTime wtime = new WorkTime();
            if (user.WorkTime != null)
            {
                wtime = new WorkTime
                {
                    MondayStart = user.WorkTime.MondayStart ?? "00:00",    //przepraszam za to
                    MondayEnd = user.WorkTime.MondayEnd ?? "00:00",
                    TuesdayStart = user.WorkTime.TuesdayStart ?? "00:00",
                    TuesdayEnd = user.WorkTime.TuesdayEnd ?? "00:00",
                    WednesdayStart = user.WorkTime.WednesdayStart ?? "00:00",
                    WednesdayEnd = user.WorkTime.WednesdayEnd ?? "00:00",
                    ThursdayStart = user.WorkTime.ThursdayStart ?? "00:00",
                    ThursdayEnd = user.WorkTime.FridayStart ?? "00:00",
                    FridayEnd = user.WorkTime.FridayEnd ?? "00:00",
                    SaturdayStart = user.WorkTime.SaturdayStart ?? "00:00",
                    SaturdayEnd = user.WorkTime.SaturdayEnd ?? "00:00",
                    SundayStart = user.WorkTime.SundayStart ?? "00:00",
                    SundayEnd = user.WorkTime.SundayEnd ?? "00:00"
                };
            }

            var role = _dbContext.UserRoles.SingleOrDefault(m => m.UserId == id);
            var roleName = _dbContext.Roles.SingleOrDefault(m => m.Id == role.RoleId);

            var vm = new EditUserViewModel
            {
                Role = roleName.Name,
                Roles = GetUserRoles(),
                UserId = id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Street = user.Street,
                StreetNumber = user.StreetNumber,
                ApartmentNumber = user.ApartmentNumber,
                PostCode = user.PostCode,
                City = user.City,
                UserName = user.UserName,
                Worktime = wtime ?? null
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel vm)
        {
            var user = await GetUserById(vm.UserId);
            if (user == null)
                return RedirectToAction("Index");

            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach(var item in rolesForUser.ToList())
            {
                if(item == "Admin")
                    return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                //if (vm.Role == "Admin")
                //{
                //    vm.Email = user.Email;
                //    vm.UserName = user.UserName;
                //    vm.FirstName = user.FirstName;
                //    vm.LastName = user.LastName;
                //    vm.Street = user.Street;
                //    vm.StreetNumber = user.StreetNumber;
                //    vm.ApartmentNumber = user.ApartmentNumber;
                //    vm.PostCode = user.PostCode;
                //    vm.City = user.City;
                //    vm.Roles = GetUserRoles();
                //    return View(vm);
                //}

                var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                string id = loggedUser?.Id;
                if (id == null)
                    return NotFound();

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            await _userManager.RemoveFromRoleAsync(user, item);

                        }
                    }

                    await _userManager.AddToRoleAsync(user, vm.Role);
                    await _userManager.SetEmailAsync(user, vm.Email);

                    var history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "edit user role",
                        Table = "AspNetUserRoles",
                        Description = "UserId{" + user.Id + "} " +
                                      "Role{" + vm.Role + "}"
                    };

                    _dbContext.EventHistory.Add(history);

                    var worktimeToDelete = _dbContext.WorkTime.Where(m => m.ServicerId == vm.UserId);
                    _dbContext.WorkTime.RemoveRange(worktimeToDelete);
                    _dbContext.SaveChanges();

                    user.FirstName = vm.FirstName;
                    user.LastName = vm.LastName;
                    user.Street = vm.Street;
                    user.StreetNumber = vm.StreetNumber;
                    user.ApartmentNumber = vm.ApartmentNumber;
                    user.PostCode = vm.PostCode;
                    user.City = vm.City;

                    //user.WorkTime = vm.Worktime;
                    user.WorkTime = vm.Worktime;

                    if (user.WorkTime != null)
                    {
                        user.WorkTime.ServicerId = vm.UserId;
                        user.WorkTime.MondayStart = vm.Worktime.MondayStart ?? "00:00";   //przepraszam za to tez
                        user.WorkTime.MondayEnd = vm.Worktime.MondayEnd ?? "00:00";
                        user.WorkTime.TuesdayStart = vm.Worktime.TuesdayStart ?? "00:00";
                        user.WorkTime.TuesdayEnd = vm.Worktime.TuesdayEnd ?? "00:00";
                        user.WorkTime.WednesdayStart = vm.Worktime.WednesdayStart ?? "00:00";
                        user.WorkTime.WednesdayEnd = vm.Worktime.WednesdayEnd ?? "00:00";
                        user.WorkTime.ThursdayStart = vm.Worktime.ThursdayStart ?? "00:00";
                        user.WorkTime.ThursdayEnd = vm.Worktime.FridayStart ?? "00:00";
                        user.WorkTime.FridayEnd = vm.Worktime.FridayEnd ?? "00:00";
                        user.WorkTime.SaturdayStart = vm.Worktime.SaturdayStart ?? "00:00";
                        user.WorkTime.SaturdayEnd = vm.Worktime.SaturdayEnd ?? "00:00";
                        user.WorkTime.SundayStart = vm.Worktime.SundayStart ?? "00:00";
                        user.WorkTime.SundayEnd = vm.Worktime.SundayEnd ?? "00:00";
                    }

                    await _userManager.UpdateAsync(user);

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "edit user",
                        Table = "AspNetUsers",
                        Description = "UserId{" + user.Id + "} " +
                                      "UserName{" + user.UserName + "} " +
                                      "Email{" + vm.Email + "} " +
                                      "FirstName{" + user.FirstName + "} " +
                                      "LastName{" + user.LastName + "} " +
                                      "Street{" + user.Street + "} " +
                                      "StreetNumber{" + user.StreetNumber + "} " +
                                      "ApartmentNumber{" + user.ApartmentNumber + "} " +
                                      "PostCode{" + user.PostCode + "} " +
                                      "City{" + user.City + "}"
                    };

                    _dbContext.EventHistory.Add(history);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
            vm.Email = user.Email;
            vm.UserName = user.UserName;
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.Street = user.Street;
            vm.StreetNumber = user.StreetNumber;
            vm.ApartmentNumber = user.ApartmentNumber;
            vm.PostCode = user.PostCode;
            vm.City = user.City;
            vm.Roles = GetUserRoles();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private async Task<ApplicationUser> GetUserById(string id) =>
           await _userManager.FindByIdAsync(id);

        private SelectList GetAllRoles() =>
            new SelectList(_roleManager.Roles.OrderBy(r => r.Name));

        private SelectList GetUserRoles() =>
            new SelectList(_roleManager.Roles.OrderBy(r => r.Name).Where(x => x.Name != "Admin"));
    }
}