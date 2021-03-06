﻿using CallCenterService.Models;
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
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 3)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 1, 21, 12, 30, 20),
                        Status = "Open",
                        ClientDescription = "Nie działa system namaczania.",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 3)
                        
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 7)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 3, 12, 9, 12, 30),
                        Status = "Open",
                        ClientDescription = "Telefon się zawiesza",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 7)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 6)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 4, 9, 8, 42, 33),
                        Status = "Open",
                        ClientDescription = "W telewizorze nie działa Bluetooth",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 6)

                    });
                _dbContext.SaveChanges();
            }
            //if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 6)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 2, 11, 11, 11, 11),
                        Status = "In progress",
                        ClientDescription = "Nie można włączyć telewizora pilotem",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 6)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 2)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 5, 12, 14, 27, 53),
                        Status = "In progress",
                        ClientDescription = "Pralka przecieka",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 2)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 4)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 5, 9, 22, 10, 10),
                        Status = "In progress",
                        ClientDescription = "Zmywarka pokazuje błąd E7",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 4)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 5)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 3, 10, 9, 45, 40),
                        Status = "In progress",
                        ClientDescription = "Zmywarka nie wypuszcza saszetki",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 5)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 1)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 4, 23, 15, 15, 20),
                        Status = "Done",
                        ClientDescription = "Telewizor świeci się na zielono",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 1)
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 10)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 5, 3, 13, 20, 35),
                        Status = "Done",
                        ClientDescription = "Telefon nie wykrywa karty sim",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 10)
                    });
                _dbContext.SaveChanges();
            }
            //if ((await _dbContext.Faults.FirstOrDefaultAsync(x => x.Product.ProductID == 5)) == null)
            {
                await _dbContext.Faults.AddAsync(
                    new Fault
                    {
                        ApplicationDate = new DateTime(2018, 5, 6, 11, 10, 3),
                        Status = "Done",
                        ClientDescription = "Zmywarka pokazuje błąd E154 R4313503",
                        Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == 5)
                    });
                _dbContext.SaveChanges();
            }
        }
    }
}
