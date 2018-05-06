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
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Jan" && x.SecondName == "Kowalski" && x.Adress == "ul Jagiellońska 28, Kraków")) == null)
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Jan",
                        SecondName = "Kowalski",
                        Adress = "ul Jagiellońska 28, Kraków"

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Jan" && x.SecondName == "Kowalski" && x.Adress == "ul Czarnogórska 28, Kraków")) == null)
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Jan",
                        SecondName = "Kowalski",
                        Adress = "ul Czarnogórska 28, Kraków"

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Janina" && x.SecondName == "Kowalska" && x.Adress == "ul Nowy Sad 12, Bielsko-Biała")) == null)
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Janina",
                        SecondName = "Kowalska",
                        Adress = "ul Nowy Sad 12, Bielsko-Biała"

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Bogdan" && x.SecondName == "Nowak" && x.Adress == "ul Krzywa 24, Przemyśl") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Bodan",
                        SecondName = "Nowak",
                        Adress = "ul Krzywa 24, Przemyśl"

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Jakub" && x.SecondName == "Smoczek" && x.Adress == "ul Szarych Szeregów 15, Warszawa") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Jakub",
                        SecondName = "Smoczek",
                        Adress = "ul Szarych Szeregów 15, Warszawa"

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Krystyna" && x.SecondName == "Janda" && x.Adress == "ul Krzywe Koło 28, Wrocław") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Krystyna",
                        SecondName = "Janda",
                        Adress = "ul Krzywe Koło 28, Wrocław"

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Kamil" && x.SecondName == "Ziuta" && x.Adress == "ul Podhalańska 3, Zakopanem") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Kamil",
                        SecondName = "Ziuta",
                        Adress = "ul Podhalańska 3, Zakopanem"

                    });
                _dbContext.SaveChanges();
            }
            if ((await _dbContext.Clients.SingleOrDefaultAsync(x => x.FirstName == "Kunegunda" && x.SecondName == "Sklarczyk" && x.Adress == "ul Bystry Potok 7, Lipczynek") == null))
            {
                await _dbContext.Clients.AddAsync(
                    new Client
                    {
                        FirstName = "Kunegunda",
                        SecondName = "Sklarczyk",
                        Adress = "ul Bystry Potok 7, Lipczynek"

                    });
                _dbContext.SaveChanges();
            }

        }
    }
}
