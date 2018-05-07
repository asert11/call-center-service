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

        [Required]
        public int ProductId { get; set; }

        public List<Product> Products { get; set; }

        public AddProductFaultModel(DatabaseContext context)
        {
            Products = context.Products.ToList();
        }
    }
}
