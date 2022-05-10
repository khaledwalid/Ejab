using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.SmsMessage
{
  public class ValidateMobileViwModel
    {
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "051")]
        [Required(ErrorMessage = "020")]
        [MaxLength(12, ErrorMessage = "029")]
        public string  Mobile { get; set; }
        public string Email { get; set; }
    }
}
