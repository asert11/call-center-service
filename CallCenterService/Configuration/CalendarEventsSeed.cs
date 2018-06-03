using CallCenterService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Configuration
{
    public class CalendarEventsSeed
    {
        private readonly DatabaseContext _dbContext;

        public CalendarEventsSeed(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if ((await _dbContext.CalendarEvents.FirstOrDefaultAsync(x => x.Subject == "Testowy event 1") == null))
            {
                await _dbContext.CalendarEvents.AddAsync(
                    new CalendarEvent
                    {
                        Subject = "Testowy event 1",
                        Description = "Testowy opis 1",
                        Start = new DateTime(2018, 6, 3, 12, 30, 00),
                        End = new DateTime(2018, 6, 3, 14, 00, 00),
                        ThemeColor = "green",
                        IsFullDay = false
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.CalendarEvents.FirstOrDefaultAsync(x => x.Subject == "Testowy event 2") == null))
            {
                await _dbContext.CalendarEvents.AddAsync(
                    new CalendarEvent
                    {
                        Subject = "Testowy event 2",
                        Description = "Testowy opis 2",
                        Start = new DateTime(2018, 6, 3, 15, 30, 00),
                        End = new DateTime(2018, 6, 3, 17, 00, 00),
                        ThemeColor = "red",
                        IsFullDay = false
                    });
                _dbContext.SaveChanges();
            }
        }
    }
}
