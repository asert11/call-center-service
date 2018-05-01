using CallCenterService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.ViewModels
{
    public class SetServicerRegistrantViewModel
    {
        public Fault FaultData { get; set; }
        public List<Servicer> Servicers { get; set; }

        [Required]
        public int ServicerId { get; set; }
        [Required]
        public int FaultId { get; set; }
    }
}
