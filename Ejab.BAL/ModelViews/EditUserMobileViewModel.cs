using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class EditUserMobileViewModel
    {
        [Required( ErrorMessage ="020")]
        [Phone]
        [MaxLength (13)]
        public string  Mobile { get; set; }
    }
}
