using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class Repair
    {
        [Key]
        public int RepairId { get; set; }
        public virtual Fault Fault { get; set; }
        public virtual int FaultId { get; set; }
        public string ServicerId { get; set; }
        public String Description { get; set; }
        public DateTime?  Date { get; set; }
        public decimal Price { get; set; }
        public decimal PartsPrice { get; set; }
        public virtual CalendarEvent CalendarEvent { get; set; }

        [NotMapped]
        public ApplicationUser user { get; set; }
    }
}
