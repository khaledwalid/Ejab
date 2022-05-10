using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.Email
{
   public  class EmailSubscriptionViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
              ErrorMessageResourceName = "EmailToSubscripRequired")]
       [EmailAddress(ErrorMessageResourceType = typeof(Resources.Global),
              ErrorMessageResourceName = "EmailIsINValid")]
        //[EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
    }
}
