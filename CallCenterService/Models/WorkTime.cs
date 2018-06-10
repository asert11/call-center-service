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
       
        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")] 
        public String MondayStart { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String MondayEnd { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String TuesdayStart { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String TuesdayEnd { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String WednesdayStart { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String WednesdayEnd { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String ThursdayStart { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String ThursdayEnd { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String FridayStart { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String FridayEnd { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String SaturdayStart { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String SaturdayEnd { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String SundayStart { get; set; }

        [RegularExpression("^[0-9]:[0-5][0-9]$|^[0-1][0-9]:[0-5][0-9]$|^2[1-3]:[0-5][0-9]$", ErrorMessage = "Niepoprawne dane")]
        public String SundayEnd { get; set; }
    }
}
