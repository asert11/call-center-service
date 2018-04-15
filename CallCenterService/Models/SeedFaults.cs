using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace CallCenterService.Models
{
    public class SeedFaults
    {
        public static void InitializeFaults(IServiceProvider serviceProvider)
        {
            using (var context = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                if (context.Faults.Any())
                {
                    return;
                }
                context.Faults.AddRange(
                    new Fault
                    {
                        ClientFirstName = "Jan",
                        ClientId = 12,
                        ClientSecondName = "Kowalski",
                        Description = "Gra mi nie działa",
                        PaymentData = "12.06.2019"
                    },
                    new Fault
                    {
                        ClientFirstName = "Jane",
                        ClientId = 13,
                        ClientSecondName = "Doe",
                        Description = "Odkurzacz nie odkurza",
                        PaymentData = "12.07.2021"
                    },
                    new Fault
                    {
                        ClientFirstName = "Janina",
                        ClientId = 14,
                        ClientSecondName = "Kowalska",
                        Description = "Pralka się telepie",
                        PaymentData = "11.07.2020"
                    });
                context.SaveChanges();
            }
        }
    }
}
