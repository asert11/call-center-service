using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class Fault //: IValidatableObject
    {

        public Fault()
        {
            this.Status = "Open";
        }

        [Key]
        public int FaultId { get; set; }
        public String ClientDescription { get; set; }
        public String Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public virtual Product Product { get; set; }

        [NotMapped]
        [Required]
        public int ClientId { get; set; }

        [NotMapped]
        public List<Product> Products { get; set; }

        [NotMapped]
        [Required]
        public int ProductId { get; set; }
    }
}
