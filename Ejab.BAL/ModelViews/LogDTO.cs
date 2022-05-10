using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class LogDTO
    {
        public int Id { set; get; }
        public int? ActionId { set; get; }
        public string ActionName { set; get; }
        public string Description { set; get; }
        public int? CreatedBy { set; get; }
        public string CreatedByName { set; get; }
        public DateTime? CreatedOn { set; get; }
        public short? FlgStatus { get; set; }
    }
}
