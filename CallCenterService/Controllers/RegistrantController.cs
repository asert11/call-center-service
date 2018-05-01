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
            return View(await _context.Faults.ToListAsync());
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
            if (vm.ServicerId == 0 || ModelState.IsValid == false)
            {
                vm.Servicers = _context.Servicers.ToList();
                vm.FaultData = _context.Faults.FirstOrDefault(f => f.FaultId == vm.FaultId);

                if (vm.FaultData == null)
                {
                    return NotFound();
                }

                return View(vm);
            }

            else
            {
                ServicerFault sf = new ServicerFault
                {
                    IdFault = vm.FaultId,
                    IdServicer = vm.ServicerId
                };

                await _context.ServicerFault.AddAsync(sf);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}