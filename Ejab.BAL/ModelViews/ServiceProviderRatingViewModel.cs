using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class ServiceProviderRatingViewModel
    {
        public int RequsterId { get; set; }
        [Required(ErrorMessage = "047")]
        public int RequstId { get; set; }       
        public string  Description { get; set; }
        [Required(ErrorMessage = "050")]
        public int Rating { get; set; }
        public int OverAllRating { get; set; }
        public Double  AvgRating { get; set; }
    }
}
