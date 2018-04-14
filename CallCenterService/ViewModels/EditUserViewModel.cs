using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.ViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public SelectList Roles { get; set; }
    }
}
