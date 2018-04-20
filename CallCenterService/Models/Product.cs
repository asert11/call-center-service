using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Type { get; set; }
    }
}
