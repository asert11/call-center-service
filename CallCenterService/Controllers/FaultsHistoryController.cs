using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterService.Controllers
{
    [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]

    public class FaultsHistoryController : Controller
    {
        private readonly DatabaseContext _context;

        public FaultsHistoryController(DatabaseContext context)
        {
            _context = context;    
        }

        // GET: FaultsHistory
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faults.ToListAsync());
        }

        // GET: FaultsHistory/Details/5
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

        // GET: FaultsHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FaultsHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FaultId,ClientDescription,Status,ApplicationDate")] Fault fault)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fault);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fault);
        }

        // GET: FaultsHistory/Edit/5
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

        // POST: FaultsHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FaultId,ClientDescription,Status,ApplicationDate")] Fault fault)
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

        // GET: FaultsHistory/Delete/5
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

        // POST: FaultsHistory/Delete/5
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
