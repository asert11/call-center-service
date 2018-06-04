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
    public class ProductsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index(int ? searchIdProduct, string searchNameProduct, string searchTypeProduct)
        {

            var name = from m in _context.Products
                       select m;

            if (searchIdProduct!=null)
            {
                name = name.Where(s => s.ProductID.Equals(searchIdProduct));
            }
            if (!String.IsNullOrEmpty(searchNameProduct))
            {
                name = name.Where(s => s.Name.Contains(searchNameProduct));
            }

            if (!String.IsNullOrEmpty(searchTypeProduct))
            {
                name = name.Include(s => s.Type).Where(s => s.Type.Type.Equals(searchTypeProduct));
            }

            return View(await name.Include(m => m.Client).Include(m => m.Type).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(m => m.Type)
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product p = new Product();
            p.ClientId = (int)id;
            p.Specializations = _context.Specialization.ToList();
            return View(p);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ? id, [Bind("ProductID,Name,Type,SelectedId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Client = _context.Clients.FirstOrDefault(m => m.ClientId == id);
                product.Type = _context.Specialization.FirstOrDefault(m => m.Id == Int32.Parse(product.SelectedId));

                if (product.Type == null)
                {
                    product.Specializations = _context.Specialization.ToList();
                    return View(product);
                }

                var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                string loggedId = loggedUser?.Id;
                if (loggedId == null)
                    return NotFound();

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Add(product);
                    _context.SaveChanges();

                    var history = new EventHistory
                    {
                        Date = DateTime.Now,
                        UserId = loggedUser.Id,
                        Operation = "add product",
                        Table = "Products",
                        Description = "ProductID{" + product.ProductID + "} " +
                                      "ClientId{" + product.Client.ClientId + "} " +
                                      "Name{" + product.Name + "} " +
                                      "TypeId{" + product.Type.Id + "}"
                    };
                    _context.EventHistory.Add(history);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
            product.Specializations = _context.Specialization.ToList();
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(m => m.Type).SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Specializations = _context.Specialization.ToList();
            product.SelectedId = product.Type.Id.ToString();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Type,SelectedId")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Products.Include(m => m.Client).Where(m => m.ProductID == id).First();
                    data.Type = _context.Specialization.FirstOrDefault(m => m.Id == Int32.Parse(product.SelectedId));
                    data.Name = product.Name;
                    if(data.Type == null)
                        return RedirectToAction("Index");

                    var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                    string loggedId = loggedUser?.Id;
                    if (loggedId == null)
                        return NotFound();

                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Update(data);
                        _context.SaveChanges();

                        var history = new EventHistory
                        {
                            Date = DateTime.Now,
                            UserId = loggedUser.Id,
                            Operation = "edit product",
                            Table = "Products",
                            Description = "ProductID{" + data.ProductID + "} " +
                                      "ClientId{" + data.Client.ClientId + "} " +
                                      "Name{" + data.Name + "} " +
                                      "TypeId{" + data.Type.Id + "}"
                        };
                        _context.EventHistory.Add(history);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
            product.Specializations = _context.Specialization.ToList();
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            string loggedId = loggedUser?.Id;
            if (loggedId == null)
                return NotFound();

            EventHistory history;

            using (var transaction = _context.Database.BeginTransaction())
            {
                var product = await _context.Products.Include(m => m.Client)
                    .Include(m => m.Type).SingleOrDefaultAsync(m => m.ProductID == id);

                var faults = _context.Faults.Include(m => m.Product)
                    .Where(m => m.Product.ProductID == product.ProductID).ToList();

                foreach (var fault in faults)
                {
                    var repairs = _context.Repairs.Include(m => m.Fault)
                        .Where(m => m.Fault.FaultId == fault.FaultId).ToList();

                    foreach (var repair in repairs)
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
                
                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
