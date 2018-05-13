﻿using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CallCenterService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Specialization { get; set; }
    }
}
