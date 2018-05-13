using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;
using CallCenterService.ViewModels;

namespace CallCenterService.Controllers
{
    public class ClientsController : Controller
    {
        private readonly DatabaseContext _context;

        public ClientsController(DatabaseContext context)
        {
            _context = context;    
        }

        // GET: Clients
        public async Task<IActionResult> Index(string searchClientName, string searchClientSurname, string searchClientAddress)
        {
            var name = from m in _context.Clients
                       select m;


            if (!String.IsNullOrEmpty(searchClientName))
            {
                name = name.Where(s => s.FirstName.Equals(searchClientName));
            }

            if (!String.IsNullOrEmpty(searchClientSurname))
            {
                name = name.Where(s => s.SecondName.Equals(searchClientSurname));
            }

            if (!String.IsNullOrEmpty(searchClientAddress))
            {
                name = name.Where(s => s.Adress.Equals(searchClientAddress));
            }

            return View(await name.ToListAsync());

        }
        [HttpPost]
        public string Index(string searchClientName, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchClientName ;
        }

        // GET
        public async Task<IActionResult> History(int ? id)
        {
            ClientFaultHistoryViewModel vm = new ClientFaultHistoryViewModel();
            vm.Faults = await _context.Faults.Include(m => m.Client).Include(m => m.Product).Where(m => m.ClientId == id).ToListAsync();

            return View(vm);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .SingleOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,FirstName,SecondName,Adress")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.SingleOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,FirstName,SecondName,Adress")] Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientId))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .SingleOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(m => m.ClientId == id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}
