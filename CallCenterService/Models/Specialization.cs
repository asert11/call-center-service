using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallCenterService.Models
{
    public class Specialization //: IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Type { get; set; }

        [NotMapped]
        public bool Checked { get; set; }
    }
}
