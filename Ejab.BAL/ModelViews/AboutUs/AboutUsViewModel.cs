using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.AboutUs
{
   public  class AboutUsViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "RegionRequired")]
        public string Region { get; set; }
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
         ErrorMessageResourceName = "AddressRequired")]
        public string Address { get; set; }
      
        public double Longitude { get; set; }
        
        public double latitude { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
         ErrorMessageResourceName = "PhoneIsRequired")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "mobileNumberMustBe14Digit")]
        [StringLength(14, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "mobileNumberMustBe14Digit")]
        [MinLength(10, ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "MobileMinDigitCount")]
        public string phone { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
         ErrorMessageResourceName = "EmailIsRequired")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "InvalidEmailAddress")]
        public string Email { get; set; }
       // [Required(ErrorMessage = "76")]
        public string fax { get; set; }
    }
}
