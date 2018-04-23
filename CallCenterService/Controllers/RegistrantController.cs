using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CallCenterService.Controllers
{
    public class RegistrantController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddFault()
        {
            return RedirectToAction("Create", "Faults");
        }

        public IActionResult AddClients()
        {
            return RedirectToAction("Index", "Clients");
        }
    }
}