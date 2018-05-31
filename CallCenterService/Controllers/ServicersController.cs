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
using static CallCenterService.ViewModels.EditSpecializationViewModel;

namespace CallCenterService.Controllers
{
    public class ServicersController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ServicersController(DatabaseContext dbContext, UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string searchUserLogin, string searchUserEmail)
        {
            IList<ApplicationUser> name;

            name = await _userManager.GetUsersInRoleAsync("Serwisant");

            if (!String.IsNullOrEmpty(searchUserLogin))
            {
                name = name.Where(s => s.UserName.Equals(searchUserLogin)).ToList();
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

        [HttpGet]
        public async Task<IActionResult> Specializations(string id)
        {
            var user = await GetUserById(id);
            if (user == null)
                return RedirectToAction("Index");

            var rolesForUser = await _userManager.GetRolesAsync(user);

            bool isServicer = false;
            foreach (var item in rolesForUser.ToList())
            {
                if (item == "Admin")
                    return RedirectToAction("Index");

                if (item == "Serwisant")
                    isServicer = true;
            }

            if (!isServicer)
                return RedirectToAction("Index");

            var vm = new EditSpecializationViewModel
            {
                UserId = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Specializations = _dbContext.Specialization.ToList()
            };

            foreach(var spec in vm.Specializations)
            {
                if (_dbContext.ServicerSpecializations.FirstOrDefault(x => x.ServicerId == user.Id && x.Spec == spec) == null)
                    spec.Checked = false;
                else
                    spec.Checked = true;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Specializations(EditSpecializationViewModel vm)
        {
            var user = await GetUserById(vm.UserId);
            if (user == null)
                return RedirectToAction("Index");

            var rolesForUser = await _userManager.GetRolesAsync(user);

            bool isServicer = false;
            foreach (var item in rolesForUser.ToList())
            {
                if (item == "Admin")
                    return RedirectToAction("Index");

                if (item == "Serwisant")
                    isServicer = true;
            }

            if (!isServicer)
                return RedirectToAction("Index");

            _dbContext.ServicerSpecializations.RemoveRange(
                _dbContext.ServicerSpecializations.Where(x => x.ServicerId == user.Id));
            _dbContext.SaveChanges();

            foreach(var item in vm.Specializations)
            {
                if(item.Checked)
                {
                    var spec = _dbContext.Specialization.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (spec == null)
                        continue;

                    await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations
                    {
                        ServicerId = user.Id,
                        Spec = spec
                    });
                    _dbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        private async Task<ApplicationUser> GetUserById(string id) =>
           await _userManager.FindByIdAsync(id);
    }
}