using System.Collections.Generic;
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
    public class TimetableController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimetableController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calendar()
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
                var repairEvents = _context.Repairs.Include(m => m.CalendarEvent).Include(m => m.Fault).Where(m => m.ServicerId == id)
                    .Select(m => m.CalendarEvent).ToList();
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
                    v.ResourceId = e.ResourceId;

                    //var repair = _context.Repairs.Include(m => m.Fault).Include(m => m.FaultId).Include(m => m.CalendarEvent)
                    //    .SingleOrDefault(m => m.RepairId == v.RepairId);
                }
                var repair = _context.Repairs.Include(x => x.CalendarEvent).Include(m => m.Fault).SingleOrDefault(x => x.CalendarEvent.EventId == e.EventId);
                if (repair != null)
                {
                    repair.CalendarEvent = v;
                    repair.Date = v.Start;
                    repair.Description = v.Description;
                    repair.ServicerId = v.ResourceId;
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

        [HttpGet]
        public IActionResult GetServicersAsResources()
        {
            List<ServicersResource> resources = new List<ServicersResource>();
            var repairs = _context.Repairs.ToList();
            
            foreach (var item in repairs)
            {
                var specialization = _context.ServicerSpecializations.Include(m => m.Spec).Where(m => m.ServicerId.Equals(item.ServicerId))
                    .Select(m => m.Spec.Type).SingleOrDefault();
               // var specialization = _context.Specialization.Where(m => m.Id == specId).Select(m => m.Type).SingleOrDefault();

                ServicersResource sr = new ServicersResource
                {
                    RepairId = item.RepairId,
                    ServicerId = item.ServicerId,
                    FirstName = _context.Users.Where(m => m.Id.Equals(item.ServicerId)).Select(m => m.FirstName).SingleOrDefault(),
                    LastName = _context.Users.Where(m => m.Id.Equals(item.ServicerId)).Select(m => m.LastName).SingleOrDefault(),
                    Specialization = specialization
            };

                resources.Add(sr);
            }

            return new JsonResult(resources);
        }

        [HttpGet]
        public IActionResult GetWorktime()
        {
            var worktime = _context.WorkTime.ToList();
            return new JsonResult(worktime);
        }

        [HttpPost]
        public IActionResult DeleteRepairEvent(int id)
        {
            var status = false;

            var v = _context.CalendarEvents.Where(x => x.EventId == id).FirstOrDefault();
            var repair = _context.Repairs.Include(x => x.CalendarEvent).Where(x => x.CalendarEvent == v).SingleOrDefault();
            if (v != null)
            {
                _context.CalendarEvents.Remove(v);
                _context.Repairs.Remove(repair);
                _context.SaveChanges();
                status = true;
            }

            //ViewData["DeleteStatus"] = status;
            return new JsonResult(status);
        }
    }
}
