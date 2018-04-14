using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class Fault
    {
        public int Id { get; set; }
        public String ClientFirstName { get; set; }
        public String ClientSecondName { get; set; }
        public int ClientId { get; set; }
        public String Description { get; set; }
        public String PaymentData { get; set; }
    }
}
