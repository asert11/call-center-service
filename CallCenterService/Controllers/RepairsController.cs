using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterService.Controllers
{
    public class RepairsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public string id;

        public RepairsController(DatabaseContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Repairs
        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        public async Task<IActionResult> Index(int? searchIdRepair, string searchClientName, string searchClientSurname, string searchNameProduct)
        {
            var name = from m in _context.Repairs
                       select m;


            if (searchIdRepair != null)
            {
                name = name.Where(s => s.RepairId.Equals(searchIdRepair));
            }
            if (!String.IsNullOrEmpty(searchClientName))
            {
                name = name.Where(s => s.Fault.Product.Client.FirstName.Equals(searchClientName));
            }

            if (!String.IsNullOrEmpty(searchClientSurname))
            {
                name = name.Where(s => s.Fault.Product.Client.SecondName.Equals(searchClientSurname));
            }

            if (!String.IsNullOrEmpty(searchNameProduct))
            {
                name = name.Where(s => s.Fault.Product.Name.Contains(searchNameProduct));
            }

            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            string id = usr?.Id;
            if (id == null)
                return NotFound();

            var repairs = name.Include(r => r.Fault)
                .Include(r => r.Fault.Product).Include(r => r.Fault.Product.Client)
                .Where(s => s.Fault.Status == "In progress");

            if (!await _userManager.IsInRoleAsync(usr, "Admin") && !await _userManager.IsInRoleAsync(usr, "Kierownik"))
            {
                repairs = repairs.Where(s => s.ServicerId == id);
            }

            return View(await repairs.ToListAsync());
        }

        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        public async Task<IActionResult> Closed()
        {
            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            string id = usr?.Id;
            if (id == null)
                return NotFound();

            var repairs = _context.Repairs.Include(r => r.Fault)
                .Include(r => r.Fault.Product).Include(r => r.Fault.Product.Client)
                .Where(s => s.Fault.Status == "Done");

            if (!await _userManager.IsInRoleAsync(usr, "Admin") && !await _userManager.IsInRoleAsync(usr, "Kierownik"))
            {
                repairs = repairs.Where(s => s.ServicerId == id);
            }

            return View(await repairs.ToListAsync());
        }

        // GET: Repairs/Details/5
        [Authorize(Roles = "Admin, Kierownik, Serwisant, Rejestrujący")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(f => f.Fault)
                .Include(f => f.Fault.Product)
                .Include(f => f.Fault.Product.Client)
                .SingleOrDefaultAsync(m => m.RepairId == id);

            if (repair == null)
            {
                return NotFound();
            }
            repair.user = await _userManager.FindByIdAsync(repair.ServicerId);

            return View(repair);
        }

        // GET: Repairs/Create
        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        public async Task<IActionResult> Create([Bind("RepairId,Description,Date,Price,PartsPrice")] Repair repair)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    CalendarEvent calendarEvent = new CalendarEvent
                    {
                        Subject = repair.Fault.ClientDescription,
                        Description = repair.Description,
                        Start = (DateTime)repair.Date,
                        ThemeColor = "black",
                        IsFullDay = false
                    };
                    _context.Add(calendarEvent);
                    repair.CalendarEvent = calendarEvent;
                    _context.Add(repair);
                    await _context.SaveChangesAsync();

                    repair = _context.Repairs.Include(m => m.Fault)
                        .SingleOrDefault(m => m.RepairId == repair.RepairId);

                    var history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "add repair",
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

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                return RedirectToAction("Index");
            }
            return View(repair);
        }

        // GET: Repairs/Edit/5
        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs.Include(m => m.CalendarEvent).Include(m => m.Fault).SingleOrDefaultAsync(m => m.RepairId == id);
            repair.PriceDot = repair.Price.ToString();
            repair.PartsPriceDot = repair.PartsPrice.ToString();
            if (repair == null)
            {
                return NotFound();
            }
            return View(repair);
        }

        [HttpPost]
        public IActionResult SaveRepairEvent(CalendarEvent e)
        {
            var status = false;

            if (e.EventId > 0)
            {
                //update
                var v = _context.CalendarEvents.Where(x => x.EventId == e.EventId).FirstOrDefault();
                if (v != null)
                {
                    v.Subject = e.Subject;
                    v.Start = e.Start;
                    v.End = e.End;
                    v.Description = e.Description;
                    v.IsFullDay = e.IsFullDay;
                    v.ThemeColor = e.ThemeColor;
                    v.ResourceId = e.ResourceId;
                }
                var repair = _context.Repairs.Include(x => x.CalendarEvent).Include(m => m.Fault).SingleOrDefault(x => x.CalendarEvent.EventId == e.EventId);
                if (repair != null)
                {
                    repair.CalendarEvent = v;
                    repair.Date = v.Start;
                    repair.Description = v.Description;
                    repair.ServicerId = v.ResourceId;
                }
            }
            else
            {
                _context.CalendarEvents.Add(e);
            }
            _context.SaveChanges();
            status = true;

            // ViewData["SaveStatus"] = status;
            return new JsonResult(status);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        public async Task<IActionResult> Edit(int id, Repair repair)
        {
            var repairTmp = _context.Repairs.Include(m => m.CalendarEvent).Include(m => m.Fault).SingleOrDefault(m => m.RepairId == id);

            repairTmp.Date = repair.Date;
            repairTmp.Description = repair.Description;
           



            if (repairTmp.CalendarEvent == null)
            {
                CalendarEvent calendarEvent = new CalendarEvent
                {
                    Subject = repairTmp.Fault.ClientDescription,
                    Description = repairTmp.Description,
                    Start = (DateTime)repairTmp.Date,
                    ThemeColor = "purple",
                    IsFullDay = false,
                    ResourceId = repairTmp.ServicerId
                };

                SaveRepairEvent(calendarEvent);
                repairTmp.CalendarEvent = calendarEvent;
            }

            repairTmp.CalendarEvent.Description = repairTmp.Description;
            repairTmp.CalendarEvent.Start = (DateTime)repairTmp.Date;
            SaveRepairEvent(repairTmp.CalendarEvent);

            if (id != repair.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string priceDot = repair.PriceDot;
                if (priceDot.Contains('.'))
                {
                    priceDot = priceDot.Replace('.', ',');
                }
                decimal decimalVal = System.Convert.ToDecimal(priceDot);
                repairTmp.Price = decimalVal;


                priceDot = repair.PartsPriceDot;
                if (priceDot.Contains('.'))
                {
                    priceDot = priceDot.Replace('.', ',');
                }
                decimalVal = System.Convert.ToDecimal(priceDot);
                repairTmp.PartsPrice = decimalVal;

                try
                {
                    var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                    string loggedId = loggedUser?.Id;
                    if (loggedId == null)
                        return NotFound();

                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Update(repairTmp);
                        _context.Update(repairTmp.CalendarEvent);
                        await _context.SaveChangesAsync();

                        var history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "edit repair",
                            Table = "Repairs",
                            Description = "Date{" + repairTmp.Date + "} " +
                                      "Description{" + repairTmp.Description + "} " +
                                      "FaultId{" + repairTmp.Fault.FaultId + "} " +
                                      "PartsPrice{" + repairTmp.PartsPrice + "} " +
                                      "Price{" + repairTmp.Price + "} " +
                                      "RepairId{" + repairTmp.RepairId + "} " +
                                      "ServicerId{" + repairTmp.ServicerId + "}"
                        };
                        _context.EventHistory.Add(history);
                        _context.SaveChanges();

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.RepairId))
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
            return View(repairTmp);
        }

        // GET: Repairs/Delete/5
        [Authorize(Roles = "Admin, Kierownik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .SingleOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Kierownik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            using (var transaction = _context.Database.BeginTransaction())
            {
                EventHistory history;

                var repair = await _context.Repairs.Include(m => m.Fault)
                    .Include(m => m.Fault.Product).SingleOrDefaultAsync(m => m.RepairId == id);

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

                repair.Fault.Status = "Open";

                history = new EventHistory
                {
                    Date = DateTime.Now,
                    UserId = loggedUser.Id,
                    Operation = "edit fault",
                    Table = "Faults",
                    Description = "ApplicationDate{" + repair.Fault.ApplicationDate + "} " +
                                  "ClientDescription{" + repair.Fault.ClientDescription + "} " +
                                  "FaultId{" + repair.Fault.FaultId + "} " +
                                  "Status{" + repair.Fault.Status + "} " +
                                  "ProductID{" + repair.Fault.Product.ProductID + "}"
                };
                _context.EventHistory.Add(history);
                _context.SaveChanges();

                _context.Faults.Update(repair.Fault);

                _context.Repairs.Remove(repair);

                _context.SaveChanges();

                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        public async Task<int> CheckIfDone(int? id)
        {
            if (id == null)
            {
                return 0;
            }

            var repair = _context.Repairs.Include(m => m.CalendarEvent).SingleOrDefault(m => m.RepairId == id);

            if (repair.Price == 0)
                return 0;
            else if (repair.Date == new DateTime(0001, 01, 01, 00, 00, 00))
                return 0;

            var fault = _context.Faults.Include(m => m.Product)
                .SingleOrDefault(m => m.FaultId == repair.FaultId);
            if (fault != null)
            {
                var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                string loggedId = loggedUser?.Id;
                if (loggedId == null)
                    return 0;

                using (var transaction = _context.Database.BeginTransaction())
                {
                    fault.Status = "Done";
                    _context.Remove(repair.CalendarEvent);
                    _context.SaveChanges();

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
                    _context.SaveChanges();

                    transaction.Commit();
                }
            }

            return 1;
        }

        [Authorize(Roles = "Admin, Kierownik, Serwisant")]
        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }
    }
}
