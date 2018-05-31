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

        public async Task<IActionResult> Index(int? searchIDFault, string searchClientName, string searchClientSurname, string searchClientAddress)
        {
            var name = from m in _context.Faults
                       select m;

            if (searchIDFault!=null)
            {
                name = name.Where(s => s.FaultId.Equals(searchIDFault));
            }

            if (!String.IsNullOrEmpty(searchClientName))
            {
                name = name.Where(s => s.Product.Client.FirstName.Equals(searchClientName));
            }

            if (!String.IsNullOrEmpty(searchClientSurname))
            {
                name = name.Where(s => s.Product.Client.SecondName.Equals(searchClientSurname));
            }

            if (!String.IsNullOrEmpty(searchClientAddress))
            {
                name = name.Where(s => s.Product.Client.Street.Equals(searchClientAddress));
            }

            return View(await name.Include(f => f.Product).Include(f => f.Product.Client)
                .Where(f => f.Status.Equals("Open")).ToListAsync());
            //return View(await _context.Faults.Include(f => f.Client).Where(f => f.Status.Equals("Open")).ToListAsync());
        }

        public async Task<IActionResult> Opened_faults()
        {       
            return View(await _context.Faults.Include(f => f.Product).Include(f => f.Product.Client)
                .Where(s => s.Status.Equals("Open")).ToListAsync());
        }

        public async Task<IActionResult> assigned_faults()
        {
            return View(await _context.Faults.Include(f => f.Product).Include(f => f.Product.Client)
                .Where(s => s.Status.Equals("In progress")).ToListAsync());
        }

        public async Task<IActionResult> Closed_assigned_faults()
        {
            return View(await _context.Faults.Include(f => f.Product).Include(f => f.Product.Client)
                .Where(s => s.Status.Equals("Done")).ToListAsync());
        }

        public IActionResult FaultsList()
        {
            return RedirectToAction("", "Faults");
        }


        public IActionResult GoToRepair(int ? id)
        {
            var repair = _context.Repairs.SingleOrDefault(m => m.FaultId == id);
            int new_id = repair.RepairId;

            return RedirectToAction("Details", "Repairs", new { id = new_id});
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

            var FaultTmp = _context.Faults.Include(m => m.Product).Include(m => m.Product.Type)
                .FirstOrDefault(f => f.FaultId == id);

            var vm = new SetServicerRegistrantViewModel
            {
                FaultId = (int)id,
                FaultData = _context.Faults.FirstOrDefault(f => f.FaultId == id),
                Servicers = new List<ApplicationUser>()
            };

            var Specializations = _context.ServicerSpecializations.Include(m => m.Spec)
                .Where(m => m.Spec.Id == FaultTmp.Product.Type.Id).ToList();

            foreach(var item in Specializations)
            {
                var user = await _userManager.FindByIdAsync(item.ServicerId);
                vm.Servicers.Add(user);
            }

            if (vm.FaultData == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SetServicer(SetServicerRegistrantViewModel vm)
        {
            var fault = await _context.Faults.Include(m => m.Product).Include(m => m.Product.Type)
                .FirstOrDefaultAsync(f => f.FaultId == vm.FaultId);

            if(vm.ServicerId == null)
            {
                vm.Servicers = new List<ApplicationUser>();
                vm.FaultData = fault;

                if (vm.FaultData == null)
                {
                    return NotFound();
                }

                var Specializations = _context.ServicerSpecializations.Include(m => m.Spec)
                .Where(m => m.Spec.Id == fault.Product.Type.Id).ToList();

                foreach (var item in Specializations)
                {
                    var user = await _userManager.FindByIdAsync(item.ServicerId);
                    vm.Servicers.Add(user);
                }

                return View(vm);
            }

            var servicer = await _userManager.FindByIdAsync(vm.ServicerId);

            if (ModelState.IsValid == false || servicer == null)
            {
                vm.Servicers = new List<ApplicationUser>();
                vm.FaultData = fault;

                if (vm.FaultData == null)
                {
                    return NotFound();
                }

                var Specializations = _context.ServicerSpecializations.Include(m => m.Spec)
                .Where(m => m.Spec.Id == fault.Product.Type.Id).ToList();

                foreach (var item in Specializations)
                {
                    var user = await _userManager.FindByIdAsync(item.ServicerId);
                    vm.Servicers.Add(user);
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
                        ServicerId = servicer.Id
                    };

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