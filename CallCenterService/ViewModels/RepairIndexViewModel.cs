using CallCenterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.ViewModels
{
    public class RepairIndexViewModel : Repair
    {
        public int ClientId { get; set; }
    }
}
