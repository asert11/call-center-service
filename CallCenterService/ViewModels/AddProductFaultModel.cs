using CallCenterService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.ViewModels
{
    public class AddProductFaultModel : Fault
    {
        public List<Product> Products { get; set; }
        [Required]
        public int ProductId { get; set; }


    }
}
