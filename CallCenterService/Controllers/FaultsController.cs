using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;



namespace CallCenterService.Controllers
{
    public class FaultsController : Controller
    {
        private readonly DatabaseContext _context;

        public FaultsController(DatabaseContext context)
        {
            _context = context;    
        }

        // GET: Faults
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faults.Include(f => f.Client).ToListAsync());
        }

        // GET: Faults/Details/5
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
        public IActionResult Create(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Fault f = new Fault();
            ViewModels.AddProductFaultModel f = new ViewModels.AddProductFaultModel(_context, (int)id);
           // f.ClientId = (int)id;

            return View(f);
        }

        // POST: Faults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ? ProductId,[Bind("FaultId,ClientId,ClientDescription,Status,ApplicationDate,ProductId")] Fault fault)
        {

            if (ModelState.IsValid)
            {
                fault.Status = "Open";
                fault.ApplicationDate = DateTime.Now;
                fault.Client = _context.Clients.FirstOrDefault(m => m.ClientId == fault.ClientId);
                fault.Product = _context.Products.FirstOrDefault(m => m.ProductID == ProductId);
                _context.Add(fault);

                EventHistory ev = new EventHistory
                {
                    //Id = fault.FaultId,      // this should be autoincremnted, and it set to be, but it becomes NULL and i dont know why
                    Description = "Faults for client of Id: " + fault.FaultId + " has been added",
                    Date = System.DateTime.Now
                };

                _context.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Registrant");
            }
            return View(fault);
        }

        // GET: Faults/Edit/5
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
                    _context.Update(fault);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fault = await _context.Faults.SingleOrDefaultAsync(m => m.FaultId == id);
            _context.Faults.Remove(fault);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FaultExists(int id)
        {
            return _context.Faults.Any(e => e.FaultId == id);
        }
    }
}
