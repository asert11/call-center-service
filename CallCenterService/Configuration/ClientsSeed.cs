using CallCenterService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Configuration
{
    public class ClientsSeed
    {
        private readonly DatabaseContext _dbContext;

        public ClientsSeed(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Jan" && x.SecondName == "Kowalski") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Jan",
                        SecondName = "Kowalski",
                        Street = "Jagiellońska",
                        StreetNumber = "28",
                        PostCode = "30-006",
                        City = "Kraków"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Marek" && x.SecondName == "Rak") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Marek",
                        SecondName = "Rak",
                        Street = "Zielona",
                        StreetNumber = "28A",
                        PostCode = "33-036",
                        City = "Warszawa"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Julia" && x.SecondName == "Lachowska") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Julia",
                        SecondName = "Lachowska",
                        Street = "Polechońska",
                        StreetNumber = "7",
                        ApartmentNumber = "11",
                        PostCode = "32-123",
                        City = "Sanok"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Adam" && x.SecondName == "Nowak") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Adam",
                        SecondName = "Nowak",
                        Street = "29 Listopada",
                        StreetNumber = "31",
                        PostCode = "38-700",
                        City = "Ustrzyki Dolne"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Patrycja" && x.SecondName == "Adamska") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Patrycja",
                        SecondName = "Adamska",
                        Street = "Na Błonie",
                        StreetNumber = "11",
                        ApartmentNumber = "159",
                        PostCode = "30-147",
                        City = "Kraków"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Mirosław" && x.SecondName == "Suszek") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Mirosław",
                        SecondName = "Suszek",
                        Street = "Korczaka",
                        StreetNumber = "1B",
                        PostCode = "32-221",
                        City = "Lesko"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Karolina" && x.SecondName == "Polityńska") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Karolina",
                        SecondName = "Polityńska",
                        Street = "Długa",
                        StreetNumber = "8",
                        ApartmentNumber = "12",
                        PostCode = "39-195",
                        City = "Rzeszów"
                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Kamil" && x.SecondName == "Demkowski") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Kamil",
                        SecondName = "Demkowski",
                        Street = "Rzeczna",
                        StreetNumber = "5",
                        PostCode = "33-111",
                        City = "Rabe"
                    });
                _dbContext.SaveChanges();
            }
        }
    }
}
