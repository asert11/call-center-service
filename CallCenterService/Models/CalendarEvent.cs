using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class CalendarEvent
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public String Subject { get; set; }

        public String Description { get; set; }

        [Required]
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public String ThemeColor { get; set; }

        [Required]
        public Boolean IsFullDay { get; set; }
    }
}
