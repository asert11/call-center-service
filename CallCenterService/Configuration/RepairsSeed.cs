using CallCenterService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace CallCenterService.Configuration
{

    public class RepairsSeed
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepairsSeed(DatabaseContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            if ((await _dbContext.Repairs.FirstOrDefaultAsync(x => x.Fault.FaultId == 4)) == null)
            {
                await _dbContext.Repairs.AddAsync(
                        new Repair()
                        {
                            Fault = await _dbContext.Faults.FirstOrDefaultAsync(x => x.FaultId == 4),
                            Servicer = await _userManager.FindByNameAsync("AntoniNowak"),
                            Date = new DateTime(1534753945273948523),
                            Description = "Napraa przebiegła bez problemu",
                            PartsPrice = 10.54F,
                            Price = 24.32F
                
                        });

                _dbContext.SaveChanges();
            }
        }
    }
}
