using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual ApplicationUser Servicer { get; set; }
        public string ServicerId { get; set; }
        public String Description { get; set; }
        public DateTime?  Date { get; set; }
        public decimal Price { get; set; }
        public decimal PartsPrice { get; set; }
    }
}
