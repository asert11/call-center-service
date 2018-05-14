using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CallCenterService.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Imię powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public string FirstName { get; set; }
        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Nazwisko powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Specialization { get; set; }
    }
}
