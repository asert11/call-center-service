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
        public async Task<IActionResult> Index(int ? searchIdRepair,string searchClientName, string searchClientSurname, string searchNameProduct)
        {
            var name = from m in _context.Repairs
                       select m;


            if (searchIdRepair!=null)
            {
                name = name.Where(s => s.RepairId.Equals(searchIdRepair));
            }
            if (!String.IsNullOrEmpty(searchClientName))
            {
                name = name.Where(s => s.Fault.Client.FirstName.Equals(searchClientName));
            }

            if (!String.IsNullOrEmpty(searchClientSurname))
            {
                name = name.Where(s => s.Fault.Client.SecondName.Equals(searchClientSurname));
            }

            if (!String.IsNullOrEmpty(searchNameProduct))
            {
                name = name.Where(s => s.Fault.Product.Name.Contains(searchNameProduct));
            }





            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            string id = usr?.Id;
            if (id == null)
                return NotFound();

            var repairs = name.Include(r => r.Servicer).Include(r => r.Fault)
                .Include(r => r.Fault.Product).Include(r => r.Fault.Client).Where(s => s.Servicer.Id == id)
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
                .Include(f => f.Fault.Client)
                .Include(f => f.Fault.Product)
                .SingleOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

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

            var repair = await _context.Repairs.SingleOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Repair repair)
        {

            var repairTmp = _context.Repairs.SingleOrDefault(m => m.RepairId == id);

            repairTmp.Date = repair.Date;
            repairTmp.Description = repair.Description;
            repairTmp.Price = repair.Price;
            repairTmp.PartsPrice = repair.PartsPrice;

            if (id != repair.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                { 
                    _context.Update(repairTmp);
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

            var repair = _context.Repairs.SingleOrDefault(m => m.RepairId == id);

            if (repair.Price == 0)
                return 0;
            else if (repair.Date == new DateTime(0001, 01, 01, 00, 00, 00))
                return 0;

            var fault = _context.Faults.SingleOrDefault(m => m.FaultId == repair.FaultId);
            if (fault != null)
            {
                fault.Status = "Done";
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
