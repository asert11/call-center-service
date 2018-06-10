using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterService.Models;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;




// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallCenterService.Controllers
{
    [Authorize(Roles = "Admin, Kierownik, Księgowa")]

    public class AccountantController : Controller
    {
        private readonly DatabaseContext _context;

        public AccountantController(DatabaseContext context)
        {
            _context = context;
        }
        // GET: /<controller>/


        public async Task<IActionResult> Index()
        {
            var name = from m in _context.Repairs
                       select m;
            /*string searchServicePrice, string searchPartsPrice, string searchSummaryPrice,string searchDate
            if (!String.IsNullOrEmpty(searchServicePrice))
            {
                float parsedSearchServicePrice = float.Parse(searchServicePrice, CultureInfo.InvariantCulture.NumberFormat);
                name = name.Where(s => s.Price.Equals(parsedSearchServicePrice));
            }

            if (!String.IsNullOrEmpty(searchPartsPrice))
            {
                float parsedSearchPartsPrice = float.Parse(searchPartsPrice, CultureInfo.InvariantCulture.NumberFormat);
                name = name.Where(s => s.PartsPrice.Equals(parsedSearchPartsPrice));
            }

            if (!String.IsNullOrEmpty(searchSummaryPrice))
            {
                float parsedSearchSummaryPrice = float.Parse(searchSummaryPrice, CultureInfo.InvariantCulture.NumberFormat);
                name = name.Where(s => (s.PartsPrice + s.Price).Equals(parsedSearchSummaryPrice));
            }
            if (!String.IsNullOrEmpty(searchDate))
            {
                DateTime date = DateTime.Parse(searchDate,  CultureInfo.InvariantCulture.DateTimeFormat);
                name = name.Where(s => s.Date.Equals(searchDate));
            }*/

            return View(await name.ToListAsync());

        }
        public async Task<IActionResult> PDF()
        {
            return View();
        }
    }


}