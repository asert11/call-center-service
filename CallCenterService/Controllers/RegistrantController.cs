using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenterService.Models;
using CallCenterService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCenterService.Controllers
{
    public class RegistrantController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrantController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Faults.Include(f => f.Client).ToListAsync());
        }

        public async Task<IActionResult> assigned_faults()
        {
            var faults = from m in _context.Faults
                         select m;
            string ss = "In-Progress";
            if (!String.IsNullOrEmpty(ss))
            {
                faults = faults.Where(s => s.Status.Equals("In-Progress"));
            }
            return View(await faults.ToListAsync());
        }

        public async Task<IActionResult> Closed_assigned_faults()
        {
            var faults = from m in _context.Faults
                         select m;
            string ss = "Closed";
            if (!String.IsNullOrEmpty(ss))
            {
                faults = faults.Where(s => s.Status.Equals("Closed"));
            }
            return View(await faults.ToListAsync());
        }

        public IActionResult FaultsList()
        {
            return RedirectToAction("", "Faults");
        }


        public IActionResult AddFault()
        {
            return RedirectToAction("Create", "Faults");
        }

        [HttpGet]
        public async Task<IActionResult> SetServicer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new SetServicerRegistrantViewModel
            {
                FaultId = (int)id,
                Servicers = await _userManager.GetUsersInRoleAsync("Serwisant"),
                FaultData = _context.Faults.FirstOrDefault(f => f.FaultId == id)
            };

            if (vm.FaultData == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SetServicer(SetServicerRegistrantViewModel vm)
        {
            var fault = await _context.Faults.FirstOrDefaultAsync(f => f.FaultId == vm.FaultId);

            if(vm.ServicerId == null)
            {
                vm.Servicers = await _userManager.GetUsersInRoleAsync("Serwisant");
                vm.FaultData = fault;

                if (vm.FaultData == null)
                {
                    return NotFound();
                }

                return View(vm);
            }

            var servicer = await _userManager.FindByIdAsync(vm.ServicerId);

            if (ModelState.IsValid == false || servicer == null)
            {
                vm.Servicers = await _userManager.GetUsersInRoleAsync("Serwisant");
                vm.FaultData = fault;

                if (vm.FaultData == null)
                {
                    return NotFound();
                }

                return View(vm);
            }

            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    fault.Status = "In progress";
                    await _context.SaveChangesAsync();

                    Repair sf = new Repair
                    {
                        Fault = fault,
                        Servicer = servicer
                    };

                    //_context.ServicerFault.RemoveRange(_context.ServicerFault.Where(x => x.IdFault == vm.FaultId));

                    await _context.Repairs.AddAsync(sf);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
        }

        public IActionResult AddClients()
        {
            return RedirectToAction("Index", "Clients");
        }
    }
}