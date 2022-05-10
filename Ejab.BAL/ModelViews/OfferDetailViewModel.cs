using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public class OfferDetailViewModel
    {
       
        public int OfferId { get; set; }
        public int truckId { get; set; }
        public TrucksViewModel OfferTrucks { get; set; }
        public string  truckName { get; set; }
        public string truckNameArb { get; set; }
        public string  truckTypeName { get; set; }
        public string ParenttruckName { get; set; }
        public string ParenttruckNameArb { get; set; }
        public int trucksNo { get; set; }
        public int? Weight { get; set; }
        public string  ImagePath { get; set; }
    }
}
