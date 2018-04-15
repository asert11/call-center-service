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
            return View(await _context.Faults.ToListAsync());
        }

        // GET: Faults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fault = await _context.Faults
                .SingleOrDefaultAsync(m => m.Id == id);
            if (fault == null)
            {
                return NotFound();
            }

            return View(fault);
        }

        // GET: Faults/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientFirstName,ClientSecondName,ClientId,Description,PaymentData")] Fault fault)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fault);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
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

            var fault = await _context.Faults.SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientFirstName,ClientSecondName,ClientId,Description,PaymentData")] Fault fault)
        {
            if (id != fault.Id)
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
                    if (!FaultExists(fault.Id))
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
                .SingleOrDefaultAsync(m => m.Id == id);
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
            var fault = await _context.Faults.SingleOrDefaultAsync(m => m.Id == id);
            _context.Faults.Remove(fault);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FaultExists(int id)
        {
            return _context.Faults.Any(e => e.Id == id);
        }
    }
}
