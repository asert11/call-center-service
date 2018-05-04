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
        public virtual ApplicationUser Servicer { get; set; }
        public String Description { get; set; }
        [Required]
        public DateTime  Date { get; set; }
        public float Price { get; set; }
        public float PartsPrice { get; set; }
    }
}
