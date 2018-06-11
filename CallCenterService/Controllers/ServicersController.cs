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
    [Authorize(Roles = "Admin, Kierownik")]

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
                if (_dbContext.ServicerSpecializations.FirstOrDefault(x => x.ServicerId == user.Id && x.Spec == spec) != null)
                    vm.SpecId = spec.Id;
                    //spec.Checked = false;
                //else
                    //spec.Checked = true;
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

            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            EventHistory history;


            var servicerSpecializations = _dbContext.ServicerSpecializations
                .Include(m => m.Spec).Where(x => x.ServicerId == user.Id).ToList();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                foreach (var spec in servicerSpecializations)
                { 
                    if (spec.Spec.Id != vm.SpecId)
                    {
                        history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "delete servicer specialization",
                            Table = "ServicerSpecializations",
                            Description = "Id{" + spec.Id + "} " +
                                    "ServicerId{" + spec.ServicerId + "} " +
                                    "SpecId{" + spec.Spec.Id + "}"
                        };

                        _dbContext.EventHistory.Add(history);
                        _dbContext.ServicerSpecializations.Remove(spec);
                        _dbContext.SaveChanges();

                        break;
                    }
                }

                //_dbContext.ServicerSpecializations.RemoveRange(
                 //   _dbContext.ServicerSpecializations.Where(x => x.ServicerId == user.Id));
                _dbContext.SaveChanges();

                //foreach (var item in vm.Specializations)
                //{
                //    if (item.Checked)
                //    {
                //        var spec = _dbContext.Specialization.Where(x => x.Id == item.Id).FirstOrDefault();
                //        if (spec == null)
                //            continue;

                //        await _dbContext.ServicerSpecializations.AddAsync(new ServicerSpecializations
                //        {
                //            ServicerId = user.Id,
                //            Spec = spec
                //        });
                //        _dbContext.SaveChanges();
                //    }
                //}

                servicerSpecializations = _dbContext.ServicerSpecializations
                .Include(m => m.Spec).Where(x => x.ServicerId == user.Id).ToList();

                bool add = true;

                foreach (var spec in servicerSpecializations)
                {
                    if (spec.Spec.Id == vm.SpecId)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    var spec = _dbContext.Specialization.Where(x => x.Id == vm.SpecId).FirstOrDefault();
                    if (spec != null)
                    {
                        var data = new ServicerSpecializations
                        {
                            ServicerId = user.Id,
                            Spec = spec
                        };

                        await _dbContext.ServicerSpecializations.AddAsync(data);
                        _dbContext.SaveChanges();

                        history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "add servicer specialization",
                            Table = "ServicerSpecializations",
                            Description = "Id{" + data.Id + "} " +
                                        "ServicerId{" + data.ServicerId + "} " +
                                        "SpecId{" + data.Spec.Id + "}"
                        };

                        _dbContext.EventHistory.Add(history);
                        _dbContext.SaveChanges();
                    }
                }

                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        private async Task<ApplicationUser> GetUserById(string id) =>
           await _userManager.FindByIdAsync(id);
    }
}