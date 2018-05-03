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

        public IActionResult Index()
        {
            var vm = new List<IndexRegistrantViewModel>();

            var faults = _context.Faults.ToList();
            foreach(var fault in faults)
            {
                var ivm = new IndexRegistrantViewModel();
                ivm.FaultData = fault;

                var sf = _context.ServicerFault.FirstOrDefault(f => f.IdFault == fault.FaultId);
                if(sf == null)
                {
                    ivm.Servicer = new Servicer();
                    ivm.Servicer.FirstName = "";
                    ivm.Servicer.SecondName = "";
                    ivm.Servicer.Specialization = "";
                }
                else
                {
                    var servicer = _context.Servicers.FirstOrDefault(f => f.ServicerId == sf.IdServicer);
                    if (servicer == null)
                    {
                        ivm.Servicer = new Servicer();
                        ivm.Servicer.FirstName = "";
                        ivm.Servicer.SecondName = "";
                        ivm.Servicer.Specialization = "";
                    }
                    else
                        ivm.Servicer = servicer;
                }
                vm.Add(ivm);
            }

            return View(vm);
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

                _context.ServicerFault.RemoveRange(_context.ServicerFault.Where(x => x.IdFault == vm.FaultId));

                await _context.ServicerFault.AddAsync(sf);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        public IActionResult AddClients()
        {
            return RedirectToAction("Index", "Clients");
        }
    }
}