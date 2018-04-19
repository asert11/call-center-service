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


        public async Task<IActionResult> Index()
        {
            return View(await _context.Faults.ToListAsync());
        }


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


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientFirstName,ClientSecondName,ClientId,Description,PaymentData")] Fault fault)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fault);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Registrant");
            }
            return View(fault);
        }


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
