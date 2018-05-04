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
            .SingleOrDefaultAsync(m => m.FaultId == id);
        if (fault == null)
        {
            return NotFound();
        }

<<<<<<< HEAD
        return View(fault);
    }

    // GET: Faults/Create
    public IActionResult Create(int? id)
    {
        if (id == null)
        {
            return NotFound();
=======
        // GET: Faults/Create
        public IActionResult Create(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Fault f = new Fault();
            f.ClientId = (int)id;

            return View(f);
>>>>>>> 5173cc41a0b3b27cede398ebbbd38f6651c68e14
        }

        AddProductFaultModel f = new AddProductFaultModel
        {
            Products = _context.Products.ToList(),
        };

        f.ClientId = (int)id;


        return View(f);
    }

    // POST: Faults/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FaultId,ClientId,ClientDescription,Status,ApplicationDate, ProductId")] AddProductFaultModel fault)
    {
            if (ModelState.IsValid && fault.ProductId != 0)
            {

                var Product = await _context.Products.FirstOrDefaultAsync(s => s.ProductID == fault.ProductId);

                fault.Product = Product;

                fault.Status = "Open";
                fault.ApplicationDate = DateTime.Now;

                Fault f = new Fault
                {
                    ClientId = fault.ClientId,
                    ClientDescription = fault.ClientDescription,
                    Status = fault.Status,
                    ApplicationDate = fault.ApplicationDate,
                    Client = fault.Client,
                    Product = fault.Product

                };

            _context.Add(f);

            EventHistory ev = new EventHistory
            {
                //Id = fault.FaultId,      // this should be autoincremnted, and it set to be, but it becomes NULL and i dont know why
                Description = "Faults for client of Id: " + fault.FaultId + " has been added",
                Date = System.DateTime.Now
            };

            _context.Add(ev);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        fault.Products = _context.Products.ToList();

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



    /*  public async Task<IActionResult> SetProduct(AddProductFaultModel vm)
      {
          var product = await _context.Products.FirstOrDefaultAsync(s => s.ProductID == vm.ProductId);
          var fault = await _context.Faults.FirstOrDefaultAsync(f => f.FaultId == vm.FaultId);

          if (vm.ProductId == 0 || ModelState.IsValid == false || product == null)
          {
              vm.Products = _context.Products.ToList();
              vm.FaultData = fault;

              if (vm.FaultData == null)
              {
                  return NotFound();
              }

              return View(vm);
          }

          else
          {
              using (var transaction = _context.Database.BeginTransaction())
              {
                  fault.Status = "In progress";
                  await _context.SaveChangesAsync();

                  Repair sf = new Repair
                  {
                      Fault = fault

                  };

                  //_context.ServicerFault.RemoveRange(_context.ServicerFault.Where(x => x.IdFault == vm.FaultId));

                  await _context.Repairs.AddAsync(sf);
                  await _context.SaveChangesAsync();

                  transaction.Commit();
              }

              return RedirectToAction("Index");
          }
      }*/


}
}
