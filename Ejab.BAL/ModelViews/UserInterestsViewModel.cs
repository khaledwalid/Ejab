using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class UserInterestsViewModel
    {
     // public int InterestId { get; set; }
        //public virtual TrucksViewModel Truck { get; set; }
        //public virtual RegionModelView Region{ get; set; }

        public virtual  IEnumerable< InterestViewModel   > Trucks { get; set; }
        public virtual  IEnumerable<InterestViewModel> Regions { get; set; }
    }
}
