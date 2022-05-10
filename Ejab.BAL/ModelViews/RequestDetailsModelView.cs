using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public   class RequestDetailsModelView
    {
        public int Id { get; set; }
        public int RequestId { get; set; } // OfferId    
       public  TrucksViewModel requestTrucks { get; set; }
        public string   trucksType { get; set; }
        public int truckId { get; set; }
        public string truckName { get; set; }
        public string truckNameArb { get; set; }
        public string truckParentName { get; set; }
        public string truckParentNameArb { get; set; }
        public string trucksTypeNameArb { get; set; }
        public int trucksNo { get; set; }
        public string  trucksImagePath { get; set; }
        public decimal Price { get; set; }
        public DateTime? Expiredate { get; set; }
        public string  Notes { get; set; }
        public IEnumerable<RequestDetailesPricesViewModel> PricingDetailes { get; set; }

    }
}
