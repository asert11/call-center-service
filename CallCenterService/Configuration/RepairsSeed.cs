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
                            Description = "Naprawa przebiegła bez problemu",
                            PartsPrice = 10.54F,
                            Price = 24.32F
                
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
                            Date = new DateTime(1134753945273948523),
                            Description = "Wymieniono uszczelke, problem zniknął",
                            PartsPrice = 12.74F,
                            Price = 124.32F

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
                            Date = new DateTime(1334753945273948523),
                            Description = "Wymiana pompy wodnej",
                            PartsPrice = 25.74F,
                            Price = 86.32F

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
                            Date = new DateTime(124753945273948523),
                            Description = "Naprawa systemu podawania tabletki",
                            PartsPrice = 125.74F,
                            Price = 386.32F

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
                            Date = new DateTime(114753945273948523),
                            Description = "Wymiana wyświetlacza",
                            PartsPrice = 525.74F,
                            Price = 786.32F

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
                            Date = new DateTime(114753945273948523),
                            Description = "Wymiana gniazda karty sim",
                            PartsPrice = 25.74F,
                            Price = 125.32F

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
                            Date = new DateTime(14753945273948523),
                            Description = "Wyczyszczenie zapchanego filtra",
                            PartsPrice = 22.74F,
                            Price = 29.32F

                        });

                _dbContext.SaveChanges();
            }
        }
    }
}
