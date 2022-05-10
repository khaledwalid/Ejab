using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class AcceptofferViewModel
    {
        public int OfferId { get; set; } // OfferId
        public OfferViewModel Offer { get; set; }
        [Required(ErrorMessage = "062")]
        public int AcceptedUserId { get; set; } // AcceptedUserId
        public string AcceptedUserName { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "031")]
        public System.DateTime? AcceptedDate { get; set; } // AcceptedDate
        public string Notes { get; set; } // Notes (length: 250)
        public OfferState OfferState { get; set; }
    }
}
