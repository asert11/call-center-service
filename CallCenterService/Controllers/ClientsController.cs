using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;
using CallCenterService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterService.Controllers
{
    public class ClientsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientsController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Clients
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
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
                name = name.Where(s => s.Street.Equals(searchClientAddress));
            }

            return View(await name.ToListAsync());

        }
        [HttpPost]
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
        public string Index(string searchClientName, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchClientName ;
        }

        // GET
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
        public async Task<IActionResult> History(int ? id)
        {
            ClientFaultHistoryViewModel vm = new ClientFaultHistoryViewModel();
            vm.Faults = await _context.Faults.Include(m => m.Product).Where(m => m.Product.ClientId == id).ToListAsync();

            return View(vm);
        }

        // GET: Clients/Details/5
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
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
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
        public async Task<IActionResult> Create(
            [Bind("ClientId,FirstName,SecondName,Street,StreetNumber,ApartmentNumber,City,PostCode")] Client client)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Add(client);
                    await _context.SaveChangesAsync();

                    var history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "add client",
                        Table = "Clients",
                        Description = "ClientId{" + client.ClientId + "} " +
                                      "FirstName{" + client.FirstName + "} " +
                                      "SecondName{" + client.SecondName + "} " +
                                      "Street{" + client.Street + "} " +
                                      "StreetNumber{" + client.StreetNumber + "} " +
                                      "ApartmentNumber{" + client.ApartmentNumber + "} " +
                                      "PostCode{" + client.PostCode + "} " +
                                      "City{" + client.City + "}"
                    };

                    _context.EventHistory.Add(history);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
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
        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
        public async Task<IActionResult> Edit(int id, 
            [Bind("ClientId,FirstName,SecondName,Street,StreetNumber,ApartmentNumber,City,PostCode")] Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Update(client);
                        await _context.SaveChangesAsync();

                        var history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "edit client",
                            Table = "Clients",
                            Description = "ClientId{" + client.ClientId + "} " +
                                      "FirstName{" + client.FirstName + "} " +
                                      "SecondName{" + client.SecondName + "} " +
                                      "Street{" + client.Street + "} " +
                                      "StreetNumber{" + client.StreetNumber + "} " +
                                      "ApartmentNumber{" + client.ApartmentNumber + "} " +
                                      "PostCode{" + client.PostCode + "} " +
                                      "City{" + client.City + "}"
                        };

                        _context.EventHistory.Add(history);
                        _context.SaveChanges();

                        transaction.Commit();
                    }
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
        [Authorize(Roles = "Admin, Kierownik")]
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
        [Authorize(Roles = "Admin, Kierownik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            EventHistory history;

            using (var transaction = _context.Database.BeginTransaction())
            {
                var client = await _context.Clients.SingleOrDefaultAsync(m => m.ClientId == id);

                var products = _context.Products.Include(m => m.Client).Include(m => m.Type)
                    .Where(m => m.Client.ClientId == id).ToList();

                foreach (var product in products)
                {
                    var faults = _context.Faults.Include(m => m.Product)
                        .Where(m => m.Product.ProductID == product.ProductID).ToList();

                    foreach (var fault in faults)
                    {
                        var repairs = _context.Repairs.Include(m => m.Fault)
                            .Where(m => m.Fault.FaultId == fault.FaultId).ToList();

                        foreach(var repair in repairs)
                        {
                            history = new EventHistory
                            {
                                Date = DateTime.Now,
                                UserId = loggedUser.Id,
                                Operation = "delete repair",
                                Table = "Repairs",
                                Description = "Date{" + repair.Date + "} " +
                                      "Description{" + repair.Description + "} " +
                                      "FaultId{" + repair.Fault.FaultId + "} " +
                                      "PartsPrice{" + repair.PartsPrice + "} " +
                                      "Price{" + repair.Price + "} " +
                                      "RepairId{" + repair.RepairId + "} " +
                                      "ServicerId{" + repair.ServicerId + "}"
                            };
                            _context.EventHistory.Add(history);
                            _context.SaveChanges();

                            _context.Repairs.Remove(repair);
                        }
                        //_context.Repairs.RemoveRange(repairs);

                        history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "delete fault",
                            Table = "Faults",
                            Description = "ApplicationDate{" + fault.ApplicationDate + "} " +
                                     "ClientDescription{" + fault.ClientDescription + "} " +
                                     "FaultId{" + fault.FaultId + "} " +
                                     "Status{" + fault.Status + "} " +
                                     "ProductID{" + fault.Product.ProductID + "}"
                        };
                        _context.EventHistory.Add(history);
                        _context.SaveChanges();

                        _context.Faults.Remove(fault);
                    }

                    history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "delete product",
                        Table = "Products",
                        Description = "ProductID{" + product.ProductID + "} " +
                                      "ClientId{" + product.Client.ClientId + "} " +
                                      "Name{" + product.Name + "} " +
                                      "TypeId{" + product.Type.Id + "}"
                    };
                    _context.EventHistory.Add(history);
                    _context.SaveChanges();

                    _context.Products.Remove(product);
                }

                history = new EventHistory
                {
                    Date = DateTime.Now,
                    UserId = loggedUser.Id,
                    Operation = "delete client",
                    Table = "Clients",
                    Description = "ClientId{" + client.ClientId + "} " +
                                      "FirstName{" + client.FirstName + "} " +
                                      "SecondName{" + client.SecondName + "} " +
                                      "Street{" + client.Street + "} " +
                                      "StreetNumber{" + client.StreetNumber + "} " +
                                      "ApartmentNumber{" + client.ApartmentNumber + "} " +
                                      "PostCode{" + client.PostCode + "} " +
                                      "City{" + client.City + "}"
                };

                _context.EventHistory.Add(history);
                _context.SaveChanges();

                _context.Clients.Remove(client);

                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Kierownik, Rejestrujący")]
        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}
