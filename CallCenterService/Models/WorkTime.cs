using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class WorkTime
    {
        public int WorkTimeId { get; set; }
        public String ServicerId { get; set; }
        public String MondayStart { get; set; }
        public String MondayEnd { get; set; }
        public String TuesdayStart { get; set; }
        public String TuesdayEnd { get; set; }
        public String WednesdayStart { get; set; }
        public String WednesdayEnd { get; set; }
        public String ThursdayStart { get; set; }
        public String ThursdayEnd { get; set; }
        public String FridayStart { get; set; }
        public String FridayEnd { get; set; }
        public String SaturdayStart { get; set; }
        public String SaturdayEnd { get; set; }
        public String SundayStart { get; set; }
        public String SundayEnd { get; set; }
    }
}
