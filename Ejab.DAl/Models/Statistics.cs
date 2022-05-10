using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Models
{
 public class Statistics:BaseModel 
    {
        public int TrucksOrdersNo { get; set; }
        public int CustomerNo { get; set; }
        public int OfferNo { get; set; }
        public int AppDownloadsNo { get; set; }
    }
}
