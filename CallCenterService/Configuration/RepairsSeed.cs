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
                            Date = new DateTime(2018, 5, 12),
                            Description = "Naprawa przebiegła bez problemu",
                            PartsPrice = 10.54m,
                            Price = 24.32m
                
                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Repairs.FirstOrDefaultAsync(x => x.Fault.FaultId == 5)) == null)
            {
                await _dbContext.Repairs.AddAsync(
                        new Repair()
                        {
                            Fault = await _dbContext.Faults.FirstOrDefaultAsync(x => x.FaultId == 5),
                            Servicer = await _userManager.FindByNameAsync("JanPrus"),
                            Date = new DateTime(2018, 11, 11),
                            Description = "Wymieniono uszczelke, problem zniknął",
                            PartsPrice = 12.74m,
                            Price = 124.32m

                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Repairs.FirstOrDefaultAsync(x => x.Fault.FaultId == 6)) == null)
            {
                await _dbContext.Repairs.AddAsync(
                        new Repair()
                        {
                            Fault = await _dbContext.Faults.FirstOrDefaultAsync(x => x.FaultId == 6),
                            Servicer = await _userManager.FindByNameAsync("KonradKania"),
                            Date = new DateTime(2018, 3, 28),
                            Description = "Wymiana pompy wodnej",
                            PartsPrice = 25.74m,
                            Price = 86.32m

                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Repairs.FirstOrDefaultAsync(x => x.Fault.FaultId == 7)) == null)
            {
                await _dbContext.Repairs.AddAsync(
                        new Repair()
                        {
                            Fault = await _dbContext.Faults.FirstOrDefaultAsync(x => x.FaultId == 7),
                            Servicer = await _userManager.FindByNameAsync("KamilBem"),
                            Date = new DateTime(2017, 1, 12),
                            Description = "Naprawa systemu podawania tabletki",
                            PartsPrice = 125.74m,
                            Price = 386.32m

                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Repairs.FirstOrDefaultAsync(x => x.Fault.FaultId == 8)) == null)
            {
                await _dbContext.Repairs.AddAsync(
                        new Repair()
                        {
                            Fault = await _dbContext.Faults.FirstOrDefaultAsync(x => x.FaultId == 8),
                            Servicer = await _userManager.FindByNameAsync("ArturKopytko"),
                            Date = new DateTime(2017, 8, 23),
                            Description = "Wymiana wyświetlacza",
                            PartsPrice = 525.74m,
                            Price = 786.32m

                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Repairs.FirstOrDefaultAsync(x => x.Fault.FaultId == 9)) == null)
            {
                await _dbContext.Repairs.AddAsync(
                        new Repair()
                        {
                            Fault = await _dbContext.Faults.FirstOrDefaultAsync(x => x.FaultId == 9),
                            Servicer = await _userManager.FindByNameAsync("ArturKopytko"),
                            Date = new DateTime(2018, 2, 17),
                            Description = "Wymiana gniazda karty sim",
                            PartsPrice = 25.74m,
                            Price = 125.32m

                        });

                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Repairs.FirstOrDefaultAsync(x => x.Fault.FaultId == 10)) == null)
            {
                await _dbContext.Repairs.AddAsync(
                        new Repair()
                        {
                            Fault = await _dbContext.Faults.FirstOrDefaultAsync(x => x.FaultId == 10),
                            Servicer = await _userManager.FindByNameAsync("JanPrus"),
                            Date = new DateTime(2018, 4, 22),
                            Description = "Wyczyszczenie zapchanego filtra",
                            PartsPrice = 22.74m,
                            Price = 29.32m

                        });

                _dbContext.SaveChanges();
            }
        }
    }
}
