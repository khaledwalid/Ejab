using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class AdminviewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "UserNameRequired")]        
        public string FirstName { get; set; } // Name (length: 50)
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName ="UserLastNameRequired")]
        public string LastName { get; set; } // Name (length: 50)
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "EmailIsINValid")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "EmailIsRequired")]
        //[Display(Name = "Email", ResourceType = typeof(Resources.Global))]
        public string Email { get; set; } // Email (length: 50)
       
        //[RegularExpression(@" ^ (?:\d{8}|00\d{10}|\+\d{2}\d{8})$", ErrorMessage = "051")]
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "PhoneIsRequired")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "mobileNumberMustBe14Digit")]
        [StringLength(14, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "mobileNumberMustBe14Digit")]
        [MinLength(10, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "MobileMinDigitCount")]
        //[Display(Name = "Mobile", ResourceType = typeof(Resources.Global))]
        public string Mobile { get; set; } // Moble (length: 15)       
        public string Address { get; set; } // Address (length: 100)
        public CustomerTypes CustomerType { get; set; } // CustomerTypeId
        public decimal? AddressLatitude { get; set; } // AddressLatitude
        public decimal? AddressLongitude { get; set; } // AddressLongitude
     
        public string ProfileImgPath { get; set; }
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Password is required")]
        //[MinLength(6)]
        //[Display(Name = "Password", ResourceType = typeof(Resources.Global))]
        //[Required(ErrorMessageResourceType = typeof(Resources.Global),
        //  ErrorMessageResourceName = "PasswordIsrequired")]
        public string Password { get; set; } //User Password     
      
        public bool IsActive { get; set; }
      
        public IEnumerable<RuleViewModel> Rules { get; set; }
        public IEnumerable<RuleViewModel> ExistedRules { get; set; }
        public short FlgStatus { get; set; }
    }
}
