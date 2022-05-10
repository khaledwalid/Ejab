using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class AllMessagesViewModel
    {     
      public IEnumerable<LastMessageViewModel> SentMessages { get; set; }
        public IEnumerable<LastMessageViewModel> RecivedMessages { get; set; }
        public string  type { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public int? RequestId { get; set; }
        public int? OfferId { get; set; }
        public int UserId { get; set; }
    }

}
