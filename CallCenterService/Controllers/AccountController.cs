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
    public class AccountController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(DatabaseContext dbContext, UserManager<ApplicationUser> userManager, 
                                 SignInManager<ApplicationUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Login Page";
            return View();
        }

        [HttpPost]
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
            var vm = new AddUserViewModel();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = vm.Email, Email = vm.Email };
                var result = await _userManager.CreateAsync(user, vm.Password);

                return RedirectToAction("Users");
            }
            return View(vm);
        }

        public IActionResult Users()
        {
            var vm = new UsersViewModel()
            {
                Users = _dbContext.Users.OrderBy(u => u.Email).ToList()
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

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }

                await _userManager.DeleteAsync(user);
                transaction.Commit();
            }

            return RedirectToAction("Users");
        }

        private async Task<ApplicationUser> GetUserById(string id) =>
           await _userManager.FindByIdAsync(id);
    }
}