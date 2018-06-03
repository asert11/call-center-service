using System.Linq;
using CallCenterService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCenterService.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetRepairEvents()
        {
            var events = _context.RepairEvents.ToList();
            return new JsonResult(events);
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveRepairEvent(RepairEvent e)
        {
            var status = false;

            if (e.EventId > 0)
            {
                //update
                var v = _context.RepairEvents.Include(x => x.Repair).Where(x => x.EventId == e.EventId).FirstOrDefault();
                if (v != null)
                {
                    v.Subject = e.Subject;
                    v.Start = e.Start;
                    v.End = e.End;
                    v.Description = e.Description;
                    v.IsFullDay = e.IsFullDay;
                    v.ThemeColor = e.ThemeColor;
                    v.Repair = e.Repair;
                }
            }
            else
            {
                _context.RepairEvents.Add(e);
            }
            _context.SaveChanges();
            status = true;

           // ViewData["SaveStatus"] = status;
            return new JsonResult(status);
        }

        [HttpPost]
        public IActionResult DeleteRepairEvent(int id)
        {
            var status = false;

            var v = _context.RepairEvents.Include(x => x.Repair).Where(x => x.EventId == id).FirstOrDefault();
            if (v != null)
            {
                _context.RepairEvents.Remove(v);
                _context.SaveChanges();
                status = true;
            }

            //ViewData["DeleteStatus"] = status;
            return new JsonResult(status);
        }
    }
}
