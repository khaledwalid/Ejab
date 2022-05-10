using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class FaceBookLoginViewModel
    {      
        public string Email { get; set; } // Email (length: 50)
        public string FirstName { get; set; } // Name (length: 50)
        public string LastName { get; set; } // Name (length: 50)          
        public CustomerTypes CustomerType { get; set; }
        [Required(ErrorMessage = "015")]
        public string Token { get; set; }
        public string SN { get; set; }
        [Required(ErrorMessage = "017")]
        public string DeviceType { get; set; }
        public string ProfileImgPath { get; set; }
        public string ProfileName { get; set; }
        [Required(ErrorMessage = "052")]
        public RegisteredBy? RegisteredBy { get; set; }
        public string  FaceBookId { get; set; }
        public string Password { get; set; } //User Password
    }
}
