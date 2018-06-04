using System.Linq;
using System.Threading.Tasks;
using CallCenterService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCenterService.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IsAdmin()
        {
            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            var role = await _userManager.GetRolesAsync(usr);
            var status = false;

            if (role.Contains("Admin"))
                status = true;

            return new JsonResult(status);
        }

        [HttpGet]
        public async Task<IActionResult> GetRepairEvents()
        {
            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            string id = usr?.Id;
            if (id == null)
                return NotFound();

            var role = await _userManager.GetRolesAsync(usr);

            if (role.Contains("Serwisant"))
            {
                var repairEvents = _context.Repairs.Include(m => m.CalendarEvent).Where(m => m.ServicerId == id).Select(m => m.CalendarEvent).ToList();
                repairEvents.RemoveAll(m => m == null);
                return new JsonResult(repairEvents);
            }
          var events = _context.CalendarEvents.ToList();

            return new JsonResult(events);
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveRepairEvent(CalendarEvent e)
        {
            var status = false;

            if (e.EventId > 0)
            {
                //update
                var v = _context.CalendarEvents.Where(x => x.EventId == e.EventId).FirstOrDefault();
                if (v != null)
                {
                    v.Subject = e.Subject;
                    v.Start = e.Start;
                    v.End = e.End;
                    v.Description = e.Description;
                    v.IsFullDay = e.IsFullDay;
                    v.ThemeColor = e.ThemeColor;
                }
                var repair = _context.Repairs.Include(x => x.CalendarEvent).SingleOrDefault(x => x.CalendarEvent.EventId == e.EventId);
                if (repair != null)
                {
                    repair.CalendarEvent = v;
                    repair.Date = v.Start;
                    repair.Description = v.Description;
                }
            }
            else
            {
                _context.CalendarEvents.Add(e);
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

            var v = _context.CalendarEvents.Where(x => x.EventId == id).FirstOrDefault();
            if (v != null)
            {
                _context.CalendarEvents.Remove(v);
                _context.SaveChanges();
                status = true;
            }

            //ViewData["DeleteStatus"] = status;
            return new JsonResult(status);
        }
    }
}
