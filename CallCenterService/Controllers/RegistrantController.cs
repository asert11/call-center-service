using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenterService.Models;
using CallCenterService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCenterService.Controllers
{
    public class RegistrantController : Controller
    {
        private readonly DatabaseContext _context;

        public RegistrantController(DatabaseContext context)
        {
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
            string ss = "In progress";
            if (!String.IsNullOrEmpty(ss))
            {
                faults = faults.Where(s => s.Status.Equals("In progress"));
            }
            return View(await faults.ToListAsync());
        }

        public async Task<IActionResult> Closed_assigned_faults()
        {
            var faults = from m in _context.Faults
                         select m;
            string ss = "Done";
            if (!String.IsNullOrEmpty(ss))
            {
                faults = faults.Where(s => s.Status.Equals("Done"));
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
        public IActionResult SetServicer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new SetServicerRegistrantViewModel
            {
                FaultId = (int)id,
                Servicers = _context.Servicers.ToList(),
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
            var servicer = await _context.Servicers.FirstOrDefaultAsync(s => s.ServicerId == vm.ServicerId);
            var fault = await _context.Faults.FirstOrDefaultAsync(f => f.FaultId == vm.FaultId);

            if (vm.ServicerId == 0 || ModelState.IsValid == false || servicer == null)
            {
                vm.Servicers = _context.Servicers.ToList();
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