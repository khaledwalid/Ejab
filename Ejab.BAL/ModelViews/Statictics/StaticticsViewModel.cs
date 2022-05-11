using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.Statictics
{
   public  class StaticticsViewModel
    {
        public int Id { get; set; }
        public int TrucksOrdersNo { get; set; }
        public int CustomerNo { get; set; }
        public int OfferNo { get; set; }
        public int AppDownloadsNo { get; set; }

        public StaticticsViewModel Initialize()
        {
            TrucksOrdersNo = 0;
            CustomerNo = 0;
            OfferNo = 0;
            AppDownloadsNo = 0;
            return this;
        }
    }
}
