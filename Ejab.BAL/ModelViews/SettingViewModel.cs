using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
 public  class SettingViewModel
    {
        public int Id { get; set; }
        public System.DateTime AdminPeriod { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
            ErrorMessageResourceName = "ExpireDayies")]
        public int ExpirDayies { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "MaxExpireDayies")]
        public int MaxExpirDayies { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
            ErrorMessageResourceName = "MaxUsersNumber")]
        public int MaxAcceptNo { get; set; }
    }
}
