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
    [Authorize(Roles = "Admin")]
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
                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false);
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
                    var user = new ApplicationUser { UserName = vm.Email, Email = vm.Email };
                    var result = await _userManager.CreateAsync(user, vm.Password);
                    _dbContext.SaveChanges();

                    user = await _userManager.FindByNameAsync(vm.Email);
                    await _userManager.AddToRoleAsync(user, vm.Role);
                    _dbContext.SaveChanges();

                    transaction.Commit();

                    return RedirectToAction("Users");
                }
            }
            vm.Roles = GetUserRoles();
            return View(vm);
        }

        public IActionResult Users()
        {
            var vm = new UsersViewModel()
            {
                Users = _dbContext.Users.OrderBy(u => u.Email).Include(u => u.Roles).ToList()
            };

            return View(vm);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Users");
            }

            var user = await GetUserById(id);
            var logins = user.Logins;
            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach (var item in rolesForUser.ToList())
            {
                if (item == "Admin")
                    return RedirectToAction("Users");
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

            return RedirectToAction("Users");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await GetUserById(id);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach (var item in rolesForUser.ToList())
            {
                if (item == "Admin")
                    return RedirectToAction("Users");
            }

            var vm = new EditUserViewModel
            {
                Roles = GetUserRoles(),
                UserId = id,
                Email = user.Email
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel vm)
        {
            var user = await GetUserById(vm.UserId);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach(var item in rolesForUser.ToList())
            {
                if(item == "Admin")
                    return RedirectToAction("Users");
            }

            if (ModelState.IsValid)
            {
                if (vm.Role == "Admin")
                {
                    vm.Email = user.Email;
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

                    transaction.Commit();
                    return RedirectToAction("Users");
                }
            }
            vm.Email = user.Email;
            vm.Roles = GetUserRoles();
            return View(vm);
        }

        private async Task<ApplicationUser> GetUserById(string id) =>
           await _userManager.FindByIdAsync(id);

        private SelectList GetAllRoles() =>
            new SelectList(_roleManager.Roles.OrderBy(r => r.Name));

        private SelectList GetUserRoles() =>
            new SelectList(_roleManager.Roles.OrderBy(r => r.Name).Where(x => x.Name != "Admin"));
    }
}