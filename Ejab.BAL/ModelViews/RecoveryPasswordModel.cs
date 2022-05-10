using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class RecoveryPasswordModel
    {
        [Required(ErrorMessage ="022")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "051")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
       // [Required]
       // [DataType(DataType.Password)]
       // public string Password { get; set; }
       // // Confirm  Password
       //[DataType(DataType.Password)]
       //[Required]
       //[Display(Name = "Confirm password")]
       //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
       //public string ConfirmPassword { get; set; }
    }
}
