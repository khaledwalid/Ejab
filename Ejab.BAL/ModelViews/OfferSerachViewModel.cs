using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class OfferSearchViewModel
    {
        public string  Title { get; set; }
        public string  Description { get; set; }
        public string   Address { get; set; }
        public decimal?  PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string  search { get; set; }

    }
}
