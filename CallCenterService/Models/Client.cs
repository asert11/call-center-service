using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Imię powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public String FirstName { get; set; }
        [Required, RegularExpression("^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Nazwisko powinno zaczynać się z dużej litery i zawierać wyłącznie znaki alfabetu")]
        public String SecondName { get; set; }
        [Required]
        public String Adress { get; set; }
    }
}
