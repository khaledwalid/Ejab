using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.Reports
{
   public  class RequestViewModel
    {
        public int Id { get; set; }
        public int ReuestNumber { get; set; }      
        public string Description { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string  RegionName { get; set; }
        public string Quantity { get; set; }
        public string Period { get; set; }
        public string Notes { get; set; }
        public string Customer { get; set; }
        public string  ItemInfo { get; set; }
        public DateTime  RequestDate { get; set; }
        public string  RequestState { get; set; }

    }
}
