using System.ComponentModel.DataAnnotations;

namespace CallCenterService.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Pole login jest wymagane"), Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Pole hasło jest wymagane"), 
            MinLength(6, ErrorMessage = "Hasło powinno zawierać conajmniej 6 znaków"), 
            MaxLength(50), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
