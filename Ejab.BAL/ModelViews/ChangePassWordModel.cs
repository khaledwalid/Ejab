using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class ChangePassWordModel
    {
      
        [Required(ErrorMessage ="Old Password Required")]
        [DataType(DataType.Password)]
        public string  OldPassWord { get; set; }
        [Required(ErrorMessage ="new Password Required")]
        [DataType(DataType.Password)]
        public string NewPassWord { get; set; }
    }
}
