using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterService.Controllers
{
    public class FaultsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FaultsController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Faults
        [Authorize(Roles = "Admin, Kierownik, Rejestruj¹cy")]
        public async Task<IActionResult> Index(string searchFaultStatus, string searchClientName, string searchClientSurname, string searchClientAddress)
        {

            var name = from m in _context.Faults
                       select m;

        
            if (!String.IsNullOrEmpty(searchFaultStatus))
            {
                name = name.Where(s => s.Status.Equals(searchFaultStatus));
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

            return View(await name.Include(f => f.Product.Client).ToListAsync());
        }

        // GET: Faults/Details/5
        [Authorize(Roles = "Admin, Kierownik, Rejestruj¹cy")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .SingleOrDefaultAsync(m => m.FaultId == id);
            if (fault == null)
            {
                return NotFound();
            }

            return View(fault);
        }

        // GET: Faults/Create
        [Authorize(Roles = "Admin, Kierownik, Rejestruj¹cy")]
        public IActionResult Create(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Fault f = new Fault();
            //ViewModels.AddProductFaultModel f = new ViewModels.AddProductFaultModel(_context, (int)id);
            f.ClientId = (int)id;
            f.Products = _context.Products.Include(m => m.Client).Where(m => m.Client.ClientId == f.ClientId).ToList();

            return View(f);
        }

        // POST: Faults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Kierownik, Rejestruj¹cy")]
        public async Task<IActionResult> Create([Bind("FaultId,ClientId,ClientDescription,Status,ApplicationDate,ProductId")] Fault fault)
        {

            if (ModelState.IsValid)
            {
                fault.Status = "Open";
                fault.ApplicationDate = DateTime.Now;
                fault.Product = _context.Products.FirstOrDefault(m => m.ProductID == fault.ProductId);

                if(fault.Product == null)
                {
                    fault.Products = _context.Products.Include(m => m.Client)
                        .Where(m => m.Client.ClientId == fault.ClientId).ToList();
                    return View(fault);
                }

                var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                string loggedId = loggedUser?.Id;
                if (loggedId == null)
                    return NotFound();

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Faults.Add(fault);
                    _context.SaveChanges();

                    var history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "add fault",
                        Table = "Faults",
                        Description = "ApplicationDate{" + fault.ApplicationDate + "} " +
                                     "ClientDescription{" + fault.ClientDescription + "} " +
                                     "FaultId{" + fault.FaultId + "} " +
                                     "Status{" + fault.Status + "} " +
                                     "ProductID{" + fault.Product.ProductID + "}"
                    };

                    _context.EventHistory.Add(history);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                return RedirectToAction("Index", "Registrant");
            }
            fault.Products = _context.Products.Include(m => m.Client)
                .Where(m => m.Client.ClientId == fault.ClientId).ToList();
            return View(fault);
        }

        // GET: Faults/Edit/5
        [Authorize(Roles = "Admin, Kierownik, Rejestruj¹cy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults.SingleOrDefaultAsync(m => m.FaultId == id);
            if (fault == null)
            {
                return NotFound();
            }
            return View(fault);
        }

        // POST: Faults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Kierownik, Rejestruj¹cy")]
        public async Task<IActionResult> Edit(int id, [Bind("FaultId,ClientId,ClientDescription,Status,ApplicationDate")] Fault fault)
        {
            if (id != fault.FaultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                    string loggedId = loggedUser?.Id;
                    if (loggedId == null)
                        return NotFound();

                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Update(fault);
                        _context.SaveChanges();

                        fault = await _context.Faults.Include(m => m.Product).SingleOrDefaultAsync(m => m.FaultId == id);

                        var history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "edit fault",
                            Table = "Faults",
                            Description = "ApplicationDate{" + fault.ApplicationDate + "} " +
                                     "ClientDescription{" + fault.ClientDescription + "} " +
                                     "FaultId{" + fault.FaultId + "} " +
                                     "Status{" + fault.Status + "} " +
                                     "ProductID{" + fault.Product.ProductID + "}"
                        };

                        _context.EventHistory.Add(history);

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaultExists(fault.FaultId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(fault);
        }

        // GET: Faults/Delete/5
        [Authorize(Roles = "Admin, Kierownik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .SingleOrDefaultAsync(m => m.FaultId == id);
            if (fault == null)
            {
                return NotFound();
            }

            return View(fault);
        }

        // POST: Faults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Kierownik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            EventHistory history;

            using (var transaction = _context.Database.BeginTransaction())
            {
                var fault = await _context.Faults.Include(m => m.Product)
                    .SingleOrDefaultAsync(m => m.FaultId == id);

                var repairs = _context.Repairs.Include(m => m.Fault)
                    .Where(m => m.Fault.FaultId == fault.FaultId).ToList();

                foreach(var repair in repairs)
                {
                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete repair",
                        Table = "Repairs",
                        Description = "Date{" + repair.Date + "} " +
                                      "Description{" + repair.Description + "} " +
                                      "FaultId{" + repair.Fault.FaultId + "} " +
                                      "PartsPrice{" + repair.PartsPrice + "} " +
                                      "Price{" + repair.Price + "} " +
                                      "RepairId{" + repair.RepairId + "} " +
                                      "ServicerId{" + repair.ServicerId + "}"
                    };
                    _context.EventHistory.Add(history);
                    _context.SaveChanges();

                    _context.Repairs.Remove(repair);
                }

                //_context.Repairs.RemoveRange(repairs);

                history = new EventHistory
                {
                    Date = DateTime.Now,
                    UserId = loggedUser.Id,
                    Operation = "delete fault",
                    Table = "Faults",
                    Description = "ApplicationDate{" + fault.ApplicationDate + "} " +
                                     "ClientDescription{" + fault.ClientDescription + "} " +
                                     "FaultId{" + fault.FaultId + "} " +
                                     "Status{" + fault.Status + "} " +
                                     "ProductID{" + fault.Product.ProductID + "}"
                };
                _context.EventHistory.Add(history);
                _context.SaveChanges();

                _context.Faults.Remove(fault);
                    
                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Kierownik, Rejestruj¹cy")]
        private bool FaultExists(int id)
        {
            return _context.Faults.Any(e => e.FaultId == id);
        }
    }
}
