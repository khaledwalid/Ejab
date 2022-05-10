using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
   public  class AboutUs:BaseModel
    {
        [Required(ErrorMessage = "Region Is Required")]
        public string Region { get; set; }
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "021")]
        public string Address { get; set; }
        [Required(ErrorMessage = "78")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "79")]
        public double latitude { get; set; }
        [Required(ErrorMessage = "76")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "051")]
        public string phone { get; set; }
        [Required(ErrorMessage = "022")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "23")]
        public string Email { get; set; }
        // [Required(ErrorMessage = "76")]
        public string fax { get; set; }
    }
}
