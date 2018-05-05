using CallCenterService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Configuration
{
    public class FaultsSeed
    {
        private readonly DatabaseContext _dbContext;

        public FaultsSeed(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 1 && x.Product.ProductID == 3)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 1,
                        ApplicationDate = new DateTime(10000000000000),
                        Status = "Open",
                        ClientDescription = "Nie działa system namaczania.",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 3)
                        
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 4 && x.Product.ProductID == 7)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 4,
                        ApplicationDate = new DateTime(19430000000000),
                        Status = "Open",
                        ClientDescription = "Telefon się zawiesza",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 7)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 5 && x.Product.ProductID == 6)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 5,
                        ApplicationDate = new DateTime(10000000000000),
                        Status = "Open",
                        ClientDescription = "W telewizorze nie działa Bluetooth",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 6)

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 2 && x.Product.ProductID == 6)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 2,
                        ApplicationDate = new DateTime(17430000000000),
                        Status = "In progress",
                        ClientDescription = "Nie można włączyć telewizora pilotem",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 6)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 4 && x.Product.ProductID == 2)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 4,
                        ApplicationDate = new DateTime(17310000000000),
                        Status = "In progress",
                        ClientDescription = "Pralka przecieka",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 2)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 2 && x.Product.ProductID == 4)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 2,
                        ApplicationDate = new DateTime(10000000000000),
                        Status = "In progress",
                        ClientDescription = "Zmywarka pokazuje błąd E7",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 4)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 3 && x.Product.ProductID == 5)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 3,
                        ApplicationDate = new DateTime(10000000000000),
                        Status = "In progress",
                        ClientDescription = "Zmywarka nie wypuszcza saszetki",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 5)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 7 && x.Product.ProductID == 1)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 7,
                        ApplicationDate = new DateTime(1453200000000),
                        Status = "Done",
                        ClientDescription = "Telewizor świeci się na zielono",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 1)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 6 && x.Product.ProductID == 10)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 6,
                        ApplicationDate = new DateTime(164325000000000),
                        Status = "Done",
                        ClientDescription = "Telefon nie wykrywa karty sim",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 10)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.ClientId == 1 && x.Product.ProductID == 5)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ClientId = 1,
                        ApplicationDate = new DateTime(113634250000000000),
                        Status = "Done",
                        ClientDescription = "Zmywarka pokazuje błąd E154 R4313503",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 5)
                    });
                _dbContext.SaveChanges();
            }
        }
    }
}
