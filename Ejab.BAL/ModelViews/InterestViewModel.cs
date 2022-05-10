using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
    public class InterestViewModel
    {
        public int InterestId { get; set; }
        public int UserId { get; set; }
        public virtual UserViewModel User { get; set; }
        public int? TruckId { get; set; }
        public virtual  TrucksViewModel Truck { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int?[] RegionIds { get; set; }
        public virtual  RegionModelView Region { get; set; }
        public int Count { get; set; }
        public string  TruckName { get; set; }
        public string TruckNameArb { get; set; }
    }
}
