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
    public class SpecializationsController : Controller
    {
        private readonly DatabaseContext _context;

        public SpecializationsController(DatabaseContext context)
        {
            _context = context;    
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
                _context.Add(specialization);
                await _context.SaveChangesAsync();
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
                    _context.Update(specialization);
                    await _context.SaveChangesAsync();
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                var specialization = await _context.Specialization.SingleOrDefaultAsync(m => m.Id == id);

                var servicerSpecializations = _context.ServicerSpecializations.Include(m => m.Spec).
                    Where(m => m.Spec.Id == specialization.Id);

                _context.ServicerSpecializations.RemoveRange(servicerSpecializations);

                var products = _context.Products.Include(m => m.Type)
                    .Where(m => m.Type.Id == specialization.Id).ToList();

                foreach(var product in products)
                {
                    var faults = _context.Faults.Include(m => m.Product)
                        .Where(m => m.Product.ProductID == product.ProductID).ToList();

                    foreach(var fault in faults)
                    {
                        var repairs = _context.Repairs.Include(m => m.Fault)
                            .Where(m => m.Fault.FaultId == fault.FaultId).ToList();

                        _context.Repairs.RemoveRange(repairs);

                        _context.Faults.Remove(fault);
                    }

                    _context.Products.Remove(product);
                }

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
