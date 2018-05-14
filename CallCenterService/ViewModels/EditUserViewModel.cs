using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.ViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string Specialization { get; set; }

        [Required, EmailAddress, MaxLength(256), Display(Name = "Email Address")]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Imię powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public string FirstName { get; set; }

        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Nazwisko powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public SelectList Roles { get; set; }
    }
}
