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
    public class EventHistoryController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventHistoryController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EventHistory
        public async Task<IActionResult> Index()
        {
            var list = await _context.EventHistory.OrderByDescending(m => m.Date).ToListAsync();

            foreach(var e in list)
            {
                e.UserName = (await _userManager.FindByIdAsync(e.UserId)).UserName;
            }

            return View(list);
        }

        // GET: EventHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventHistory = await _context.EventHistory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventHistory == null)
            {
                return NotFound();
            }

            return View(eventHistory);
        }

        // GET: EventHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Table,Operation,UserId,Description,Date")] EventHistory eventHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(eventHistory);
        }

        // GET: EventHistory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventHistory = await _context.EventHistory.SingleOrDefaultAsync(m => m.Id == id);
            if (eventHistory == null)
            {
                return NotFound();
            }
            return View(eventHistory);
        }

        // POST: EventHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Table,Operation,UserId,Description,Date")] EventHistory eventHistory)
        {
            if (id != eventHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventHistoryExists(eventHistory.Id))
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
            return View(eventHistory);
        }

        // GET: EventHistory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventHistory = await _context.EventHistory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventHistory == null)
            {
                return NotFound();
            }

            return View(eventHistory);
        }

        // POST: EventHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventHistory = await _context.EventHistory.SingleOrDefaultAsync(m => m.Id == id);
            _context.EventHistory.Remove(eventHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EventHistoryExists(int id)
        {
            return _context.EventHistory.Any(e => e.Id == id);
        }
    }
}
