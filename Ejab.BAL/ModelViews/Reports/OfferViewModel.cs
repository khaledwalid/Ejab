using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.Reports
{
  public class OfferViewModel
    {
        public long  OfferNumber { get; set; }
        public string  Title { get; set; }
        public string Description { get; set; }
        public decimal  Price { get; set; }
        public string quantity { get; set; }
        public string Period { get; set; }
        public string  RegionName { get; set; }
        public string  ServiceProvider { get; set; }
        public DateTime  OfferDate { get; set; }
        public string  OfferState { get; set; }
    }
}
