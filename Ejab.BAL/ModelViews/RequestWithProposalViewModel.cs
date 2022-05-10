using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
 public    class RequestWithProposalViewModel
    {
        [Required(ErrorMessage = "047")]
        public int RequestId { get; set; } // requestId
       public string ServiceProviderName { get; set; }
        public decimal Rating { get; set; }
        public IEnumerable< RequestDetailsModelView> RequestDetails { get; set; }
        public string  TruckName { get; set; }
        public int trucksNo { get; set; }
        //public IEnumerable<>  Price { get; set; }


    }
}
