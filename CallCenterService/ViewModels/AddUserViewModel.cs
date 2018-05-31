using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CallCenterService.ViewModels
{
    public class AddUserViewModel
    {
        [Required, MinLength(6), MaxLength(256), Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, EmailAddress, MaxLength(256), Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Imię powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public string FirstName { get; set; }

        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Nazwisko powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        [RegularExpression("^[0-9]+[A-Z]?$")]
        public string StreetNumber { get; set; }

        [RegularExpression("^[0-9]*$")]
        public string ApartmentNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [RegularExpression("^[0-9]{2}-[0-9]{3}$")]
        public string PostCode { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password does not match the confirmation password.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        public SelectList Roles { get; set; }
    }
}
