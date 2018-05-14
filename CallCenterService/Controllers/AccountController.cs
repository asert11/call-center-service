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
                ModelState.AddModelError("", "Invalid Login Attempt.");
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

                using (var transaction =_dbContext.Database.BeginTransaction())
                {
                    var user = new ApplicationUser { UserName = vm.UserName, Email = vm.Email,
                        FirstName = vm.FirstName, LastName = vm.LastName, Address = vm.Address,
                        Specialization = vm.Specialization};

                    var result = await _userManager.CreateAsync(user, vm.Password);
                    _dbContext.SaveChanges();

                    if(!result.Succeeded)
                    {
                        transaction.Rollback();
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        vm.Roles = GetUserRoles();
                        return View(vm);
                    }

                    user = await _userManager.FindByNameAsync(vm.UserName);
                    await _userManager.AddToRoleAsync(user, vm.Role);
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

            var logins = user.Logins;
            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach (var item in rolesForUser.ToList())
            {
                if (item == "Admin")
                    return RedirectToAction("Index");
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }

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

            var vm = new EditUserViewModel
            {
                Roles = GetUserRoles(),
                UserId = id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                UserName = user.UserName,
                Specialization = user.Specialization
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
                if (vm.Role == "Admin")
                {
                    vm.Email = user.Email;
                    vm.UserName = user.UserName;
                    vm.FirstName = user.FirstName;
                    vm.LastName = user.LastName;
                    vm.Address = user.Address;
                    vm.Specialization = user.Specialization;
                    vm.Roles = GetUserRoles();
                    return View(vm);
                }

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

                    user.FirstName = vm.FirstName;
                    user.LastName = vm.LastName;
                    user.Address = vm.Address;
                    user.Specialization = vm.Specialization;

                    await _userManager.UpdateAsync(user);

                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
            vm.Email = user.Email;
            vm.UserName = user.UserName;
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.Address = user.Address;
            vm.Specialization = user.Specialization;
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