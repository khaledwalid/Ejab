using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class UserViewModelFromAdmin
    {
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "FirstNameRequired")]
        public string FirstName { get; set; } // Name (length: 50)
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
        ErrorMessageResourceName = "LastNameRequired")]
        [StringLength(50)]
        //[Display(Name = "LastName", ResourceType = typeof(Resources.Global))]
        public string LastName { get; set; } // Name (length: 50)
      
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "EmailIsRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "EmailIsINValid")]
        public string Email { get; set; } // Email (length: 50)

        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "mobileNumberMustBe14Digit")]     
        [StringLength(14, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "mobileNumberMustBe14Digit")]
        [MinLength (10, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "MobileMinDigitCount")]
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "PhoneIsRequired")]
        public string Mobile { get; set; } // Moble 
        public string Password { get; set; } //User Password  

        public string ProfileImgPath { get; set; }
    }
}
