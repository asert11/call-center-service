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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public String ClientDescription { get; set; }
        public String Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public virtual Client Client { get; set; }
        public virtual Product Product { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Status != "Open" || Status != "In progress" || Status != "Done")
        //    {
        //        yield return new ValidationResult
        //            ("Wrong Status!");
        //    }
        //}
    }
}
