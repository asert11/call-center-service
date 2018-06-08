using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenterService.Models
{
    public class WorkTime
    {
        public int WorkTimeId { get; set; }
        public String ServicerId { get; set; }
        [Required]
        [RegularExpression("^[0-9]$|^[0-1][0-9]$|^2[1-3]$", ErrorMessage = "Niepoprawne dane")] 
        public String MondayStart { get; set; }
        [Required]
        [RegularExpression("^[0-5][0-9]$|^[0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String MondayEnd { get; set; }
        [Required]
        [RegularExpression("^[0-9]$|^[0-1][0-9]$|^2[1-3]$", ErrorMessage = "Niepoprawne dane")]
        public String TuesdayStart { get; set; }
        [Required]
        [RegularExpression("^[0-5][0-9]$|^[0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String TuesdayEnd { get; set; }
        [Required]
        [RegularExpression("^[0-9]$|^[0-1][0-9]$|^2[1-3]$", ErrorMessage = "Niepoprawne dane")]
        public String WednesdayStart { get; set; }
        [Required]
        [RegularExpression("^[0-5][0-9]$|^[0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String WednesdayEnd { get; set; }
        [Required]
        [RegularExpression("^[0-9]$|^[0-1][0-9]$|^2[1-3]$", ErrorMessage = "Niepoprawne dane")]
        public String ThursdayStart { get; set; }
        [Required]
        [RegularExpression("^[0-5][0-9]$|^[0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String ThursdayEnd { get; set; }
        [Required]
        [RegularExpression("^[0-9]$|^[0-1][0-9]$|^2[1-3]$", ErrorMessage = "Niepoprawne dane")]
        public String FridayStart { get; set; }
        [Required]
        [RegularExpression("^[0-5][0-9]$|^[0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String FridayEnd { get; set; }
        [Required]
        [RegularExpression("^[0-9]$|^[0-1][0-9]$|^2[1-3]$", ErrorMessage = "Niepoprawne dane")]
        public String SaturdayStart { get; set; }
        [Required]
        [RegularExpression("^[0-5][0-9]$|^[0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String SaturdayEnd { get; set; }
        [Required]
        [RegularExpression("^[0-9]$|^[0-1][0-9]$|^2[1-3]$", ErrorMessage = "Niepoprawne dane")]
        public String SundayStart { get; set; }
        [Required]
        [RegularExpression("^[0-5][0-9]$|^[0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String SundayEnd { get; set; }
    }
}
