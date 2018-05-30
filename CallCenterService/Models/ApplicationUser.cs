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

        [Required]
        public String Street { get; set; }

        [Required]
        [RegularExpression("^[0-9]+[A-Z]?$")]
        public String StreetNumber { get; set; }

        [RegularExpression("^[0-9]*$")]
        public String ApartmentNumber { get; set; }

        [Required]
        public String City { get; set; }

        [Required]
        [RegularExpression("^[0-9]{2}-[0-9]{3}$")]
        public String PostCode { get; set; }

        public string Specialization { get; set; }
    }
}
