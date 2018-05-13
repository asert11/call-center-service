using CallCenterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.ViewModels
{
    public class ClientFaultHistoryViewModel
    {
        public IList<Fault> Faults { get; set; }
    }
}
