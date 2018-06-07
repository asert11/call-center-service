using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;




namespace CallCenterService.Models
{


    public class EventHistory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Table { get; set; }

        [Required]
        public string Operation { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public String Description{ get; set; }

        [Required]
        public DateTime Date { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}
