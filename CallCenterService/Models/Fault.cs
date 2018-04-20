using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class Fault : IValidatableObject
    {
        [Key]
        public int FaultId { get; set; }
        public virtual Client Client { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        public String ClientDescription { get; set; }
        [Required]
        public String Status { get; set; }
        [Required]
        public DateTime ApplicationDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status != "Open" || Status != "In progress" || Status != "Done")
            {
                yield return new ValidationResult
                    ("Wrong Status!");
            }
        }
    }
}
