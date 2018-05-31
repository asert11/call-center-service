using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class ServicerSpecializations
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ServicerId { get; set; }

        [Required]
        public virtual Specialization Spec { get; set; }
    }
}
