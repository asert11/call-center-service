using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;
using Microsoft.AspNetCore.Identity;

namespace CallCenterService.Controllers
{
    public class SpecializationsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SpecializationsController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Specializations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Specialization.ToListAsync());
        }

        // GET: Specializations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specialization
                .SingleOrDefaultAsync(m => m.Id == id);
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // GET: Specializations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specializations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                string loggedId = loggedUser?.Id;
                if (loggedId == null)
                    return NotFound();

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Add(specialization);
                    _context.SaveChanges();

                    var history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "add specialization",
                        Table = "Specialization",
                        Description = "Id{" + specialization.Id + "} " +
                                      "Type{" + specialization.Type + "}"
                    };
                    _context.EventHistory.Add(history);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                return RedirectToAction("Index");
            }
            return View(specialization);
        }

        // GET: Specializations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specialization.SingleOrDefaultAsync(m => m.Id == id);
            if (specialization == null)
            {
                return NotFound();
            }
            return View(specialization);
        }

        // POST: Specializations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] Specialization specialization)
        {
            if (id != specialization.Id)
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
                        _context.Update(specialization);
                        _context.SaveChanges();

                        var history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "edit specialization",
                            Table = "Specialization",
                            Description = "Id{" + specialization.Id + "} " +
                                      "Type{" + specialization.Type + "}"
                        };
                        _context.EventHistory.Add(history);

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecializationExists(specialization.Id))
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
            return View(specialization);
        }

        // GET: Specializations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specialization
                .SingleOrDefaultAsync(m => m.Id == id);
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // POST: Specializations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            EventHistory history;

            using (var transaction = _context.Database.BeginTransaction())
            {
                var specialization = await _context.Specialization.SingleOrDefaultAsync(m => m.Id == id);

                var servicerSpecializations = _context.ServicerSpecializations.Include(m => m.Spec).
                    Where(m => m.Spec.Id == specialization.Id).ToList();

                foreach(var servicerSpec in servicerSpecializations)
                {
                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete servicer specialization",
                        Table = "ServicerSpecializations",
                        Description = "Id{" + servicerSpec.Id + "} " +
                                      "ServicerId{" + servicerSpec.ServicerId + "} " +
                                      "SpecId{" + servicerSpec.Spec.Id + "}"
                    };

                    _context.EventHistory.Add(history);
                    _context.SaveChanges();

                    _context.ServicerSpecializations.Remove(servicerSpec);
                }

                //_context.ServicerSpecializations.RemoveRange(servicerSpecializations);

                var products = _context.Products.Include(m => m.Client)
                    .Include(m => m.Type)
                    .Where(m => m.Type.Id == specialization.Id).ToList();

                foreach(var product in products)
                {
                    var faults = _context.Faults.Include(m => m.Product)
                        .Where(m => m.Product.ProductID == product.ProductID).ToList();

                    foreach(var fault in faults)
                    {
                        var repairs = _context.Repairs.Include(m => m.Fault)
                            .Where(m => m.Fault.FaultId == fault.FaultId).ToList();

                        foreach (var repair in repairs)
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
                    }

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete product",
                        Table = "Products",
                        Description = "ProductID{" + product.ProductID + "} " +
                                      "ClientId{" + product.Client.ClientId + "} " +
                                      "Name{" + product.Name + "} " +
                                      "TypeId{" + product.Type.Id + "}"
                    };
                    _context.EventHistory.Add(history);
                    _context.SaveChanges();

                    _context.Products.Remove(product);
                }

                history = new EventHistory
                {
                    Date = DateTime.Now,
                    UserId = loggedUser.Id,
                    Operation = "delete specialization",
                    Table = "Specialization",
                    Description = "Id{" + specialization.Id + "} " +
                                      "Type{" + specialization.Type + "}"
                };
                _context.EventHistory.Add(history);
                _context.SaveChanges();

                _context.Specialization.Remove(specialization);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
            return RedirectToAction("Index");
        }

        private bool SpecializationExists(int id)
        {
            return _context.Specialization.Any(e => e.Id == id);
        }
    }
}
