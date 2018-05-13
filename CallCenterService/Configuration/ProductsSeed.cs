using CallCenterService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace CallCenterService.Configuration
{
    public class ProductsSeed
    {
        private readonly DatabaseContext _dbContext;

        public ProductsSeed(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Pralka A+++ BEKO WRE 6511 BWW")) == null){
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Pralka A+++ BEKO WRE 6511 BWW",
                            Type = "AGD",
                            ClientId = 1
                        });
                        
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Pralka GORENJE W 6503/S")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Pralka GORENJE W 6503/S",
                            Type = "AGD",
                            ClientId = 1
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Pralka BEKO WTV6533BS")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Pralka BEKO WTV6533BS",
                            Type = "AGD",
                            ClientId = 2
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Zmywarka SIEMENS SN215I01AE")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Zmywarka SIEMENS SN215I01AE",
                            Type = "AGD",
                            ClientId = 3
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Zmywarka BOSCH SMS46GI04E")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Zmywarka BOSCH SMS46GI04E",
                            Type = "AGD",
                            ClientId = 4
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Telewizor Lin 32LHD")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Telewizor Lin 32LHD",
                            Type = "RTV",
                            ClientId = 2
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Telewizor MANTA LED320E10")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Telewizor MANTA LED320E10",
                            Type = "RTV",
                            ClientId = 5
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "myPhone Prime Plus")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "myPhone Prime Plus",
                            Type = "RTV",
                            ClientId = 6
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "MaxCom MM916")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "MaxCom MM916",
                            Type = "RTV",
                            ClientId = 7
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Samsung Galaxy j5 2016 16 GB SM-J510F (Dual Sim) Złoty")) == null)
            {
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Samsung Galaxy j5 2016 16 GB SM-J510F (Dual Sim) Złoty",
                            Type = "RTV",
                            ClientId = 6
                        });

                 _dbContext.SaveChanges();
            }

        }
    }
}
