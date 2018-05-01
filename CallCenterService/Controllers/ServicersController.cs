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
    public class ServicersController : Controller
    {
        private readonly DatabaseContext _context;

        public ServicersController(DatabaseContext context)
        {
            _context = context;    
        }

        // GET: Servicers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Servicers.ToListAsync());
        }

        // GET: Servicers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicer = await _context.Servicers
                .SingleOrDefaultAsync(m => m.ServicerId == id);
            if (servicer == null)
            {
                return NotFound();
            }

            return View(servicer);
        }

        // GET: Servicers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servicers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServicerId,FirstName,SecondName,Specialization")] Servicer servicer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(servicer);
        }

        // GET: Servicers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicer = await _context.Servicers.SingleOrDefaultAsync(m => m.ServicerId == id);
            if (servicer == null)
            {
                return NotFound();
            }
            return View(servicer);
        }

        // POST: Servicers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServicerId,FirstName,SecondName,Specialization")] Servicer servicer)
        {
            if (id != servicer.ServicerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicerExists(servicer.ServicerId))
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
            return View(servicer);
        }

        // GET: Servicers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicer = await _context.Servicers
                .SingleOrDefaultAsync(m => m.ServicerId == id);
            if (servicer == null)
            {
                return NotFound();
            }

            return View(servicer);
        }

        // POST: Servicers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicer = await _context.Servicers.SingleOrDefaultAsync(m => m.ServicerId == id);
            _context.Servicers.Remove(servicer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ServicerExists(int id)
        {
            return _context.Servicers.Any(e => e.ServicerId == id);
        }
    }
}
