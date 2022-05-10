using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class AdminLogin
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "EmailIsRequired")]
        public string Email { get; set; } // Email (length: 50)
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
              ErrorMessageResourceName = "PasswordIsrequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; } //User Password
    }
}
