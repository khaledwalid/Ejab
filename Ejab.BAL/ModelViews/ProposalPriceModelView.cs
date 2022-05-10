using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public  class ProposalPriceModelView
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "047")]
        public int RequestId { get; set; } // requestId
        public RequestModelView Request { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "031")]
        public System.DateTime Date { get; set; } // Date
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "035")]
        public decimal Price { get; set; } // Price
        public int ServiceProviderId { get; set; } // ServiceProviderId
        //public string ServiceProviderName { get; set; }
        public decimal?  Rating { get; set; }
        public UserViewModel UserServiceProvider { get; set; }
        public int AcceptedBy { get; set; }
        public UserViewModel UserAcceptedBy { get; set; }
        public string AcceptedByName { get; set; }
        public bool IsAccepted { get; set; }
        public System.DateTime? ExpireDate { get; set; } // Date
        public PropsalStat PropsalStatus { get; set; }

        public string      Notes { get; set; }
        public DateTime  y1 { get; set; }
        public DateTime y2 { get; set; }
        // public int MyProperty { get; set; }




    }
}
