using System.ComponentModel.DataAnnotations;

namespace CallCenterService.ViewModels
{
    public class AddUserViewModel
    {
        [Required, EmailAddress, MaxLength(256), Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password does not match the confirmation password.")]
        public string ConfirmPassword { get; set; }
    }
}
