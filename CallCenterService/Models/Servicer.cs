using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class Servicer
    {
        [Key]
        public int ServicerId { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String SecondName { get; set; }
        [Required]
        public String Specialization { get; set; }
    }
}
