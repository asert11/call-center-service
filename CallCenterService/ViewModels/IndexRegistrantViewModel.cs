using CallCenterService.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CallCenterService.ViewModels
{
    public class IndexRegistrantViewModel
    {
        public Fault FaultData { get; set; }
        public Servicer Servicer { get; set; }
    }
}
