using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "022")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress (ErrorMessage = "023")]
        public string Email { get; set; } // Email (length: 50)
        [Required(ErrorMessage = "016")]       
        public string Password { get; set; } //User Password
        [Required(ErrorMessage = "015")]
        public CustomerTypes CustomerTypes { get; set; }
        public string Token { get; set; }       
      
    }
}
