using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
  public   class Setting : BaseModel
    {
        public System.DateTime AdminPeriod { get; set; }
        public int   ExpirDayies { get; set; }
        public int MaxExpirDayies { get; set; }
        public int MaxAcceptNo { get; set; }
    }
}
