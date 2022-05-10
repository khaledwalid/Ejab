using Ejab.BAL.CustomDTO;
using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "022")]
        [EmailAddress(ErrorMessage = "023")]
        public string Email { get; set; } // Email (length: 50)
        [Required (ErrorMessage = "018")]
       
        [StringLength(50, ErrorMessage = ("83"))]
        public string FirstName { get; set; } // Name (length: 50)
        [Required(ErrorMessage = "019")]
        [StringLength(50,ErrorMessage =("84"))]
        public string LastName { get; set; } // Name (length: 50)
        [DataType(DataType.PhoneNumber )]
        [Phone(ErrorMessage = "051")]
        [Required(ErrorMessage = "020")]
        [MaxLength  (12, ErrorMessage = "029")]
        public string Mobile { get; set; } // Moble (length: 15)
        public CustomerTypes CustomerType { get; set; }
        [Required(ErrorMessage ="016")]
        [MinLength(6, ErrorMessage = "85")]
        [DataType(DataType.Password)]
        public string Password { get; set; } //User Password
       // [Required(ErrorMessage = "015")]
        public string Token { get; set; }      
        [DataType(DataType.Password)]
      //  [MinLength(6,ErrorMessage = "85")]
        [Required(ErrorMessage ="40")]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "037")]
        public string ConfirmPassword { get; set; }
        public string ResponsiblePerson { get; set; }



    }
}
