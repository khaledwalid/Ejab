using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class ProposalPriceModelViewDetails
    {
        [Required(ErrorMessage = "047")]
        public int RequestId { get; set; } // requestId
        [Required(ErrorMessage = "031")]
        public System.DateTime Date { get; set; } // Date
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "035")]
        public decimal Price { get; set; } // Price
        public string ServiceProviderName { get; set; }
        public decimal Rating { get; set; }

    }
}
