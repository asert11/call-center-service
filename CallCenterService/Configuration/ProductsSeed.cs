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

                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");
                
                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Pralka A+++ BEKO WRE 6511 BWW",
                            Type = spec,
                            ClientId = 1
                        });
                        
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Pralka GORENJE W 6503/S")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Pralka GORENJE W 6503/S",
                            Type = spec,
                            ClientId = 1
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Pralka BEKO WTV6533BS")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Pralka BEKO WTV6533BS",
                            Type = spec,
                            ClientId = 2
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Zmywarka SIEMENS SN215I01AE")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Zmywarka SIEMENS SN215I01AE",
                            Type = spec,
                            ClientId = 3
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Zmywarka BOSCH SMS46GI04E")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "AGD");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Zmywarka BOSCH SMS46GI04E",
                            Type = spec,
                            ClientId = 4
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Telewizor Lin 32LHD")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Telewizor Lin 32LHD",
                            Type = spec,
                            ClientId = 2
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Telewizor MANTA LED320E10")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Telewizor MANTA LED320E10",
                            Type = spec,
                            ClientId = 5
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "myPhone Prime Plus")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "myPhone Prime Plus",
                            Type = spec,
                            ClientId = 6
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "MaxCom MM916")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "MaxCom MM916",
                            Type = spec,
                            ClientId = 7
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Products.SingleOrDefaultAsync(x => x.Name == "Samsung Galaxy j5 2016 16 GB SM-J510F (Dual Sim) Złoty")) == null)
            {
                var spec = _dbContext.Specialization.SingleOrDefault(x => x.Type == "RTV");

                await _dbContext.Products.AddAsync(
                        new Product()
                        {
                            Name = "Samsung Galaxy j5 2016 16 GB SM-J510F (Dual Sim) Złoty",
                            Type = spec,
                            ClientId = 6
                        });

                 _dbContext.SaveChanges();
            }

        }
    }
}
