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
    public class RepairsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public string id;

        public RepairsController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Repairs
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
                .Include(r => r.Fault.Product).Include(r => r.Fault.Product.Client).Where(s => s.ServicerId == id)
                .Where(s => s.Fault.Status == "In progress");

            return View(await repairs.ToListAsync());
        }

        // GET: Repairs/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairId,Description,Date,Price,PartsPrice")] Repair repair)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("Index");
            }
            return View(repair);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs.Include(m => m.CalendarEvent).SingleOrDefaultAsync(m => m.RepairId == id);
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
                }
            }
            else
            {
                _context.CalendarEvents.Add(e);
            }
            _context.SaveChanges();
            status = true;

            return new JsonResult(status);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Repair repair)
        {

            var repairTmp = _context.Repairs.Include(m => m.CalendarEvent).Include(m => m.Fault).SingleOrDefault(m => m.RepairId == id);

            repairTmp.Date = repair.Date;
            repairTmp.Description = repair.Description;
            repairTmp.Price = repair.Price;
            repairTmp.PartsPrice = repair.PartsPrice;

            if (repairTmp.CalendarEvent == null)
            {
                CalendarEvent calendarEvent = new CalendarEvent
                {
                    Subject = repairTmp.Fault.ClientDescription,
                    Description = repairTmp.Description,
                    Start = (DateTime)repairTmp.Date,
                    ThemeColor = "purple",
                    IsFullDay = false
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
                try
                {
                    _context.Update(repairTmp);
                    _context.Update(repairTmp.CalendarEvent);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repairs.SingleOrDefaultAsync(m => m.RepairId == id);
            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public int CheckIfDone(int? id)
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

            var fault = _context.Faults.SingleOrDefault(m => m.FaultId == repair.FaultId);
            if (fault != null)
            {
                fault.Status = "Done";
                _context.Remove(repair.CalendarEvent);
                _context.SaveChanges();
            }

            return 1;
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }
    }
}
