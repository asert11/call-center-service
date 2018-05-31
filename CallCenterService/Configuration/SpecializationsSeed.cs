using CallCenterService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Configuration
{
    public class SpecializationsSeed
    {
        private readonly DatabaseContext _dbContext;

        public SpecializationsSeed(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if ((await _dbContext.Specialization.SingleOrDefaultAsync(x => x.Type == "AGD") == null))
            {
                await _dbContext.Specialization.AddAsync(
                    new Specialization
                    {
                        Type = "AGD"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Specialization.SingleOrDefaultAsync(x => x.Type == "RTV") == null))
            {
                await _dbContext.Specialization.AddAsync(
                    new Specialization
                    {
                        Type = "RTV"
                    });
                _dbContext.SaveChanges();
            }
        }
    }
}
