using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        
        public virtual Specialization Type { get; set; }

        public virtual int ClientId { get; set; }
        public virtual Client Client { get; set; }
        
        [NotMapped]
        public List<Specialization> Specializations { get; set; }

        [NotMapped]
        [Required]
        public string SelectedId { get; set; }
    }
}
