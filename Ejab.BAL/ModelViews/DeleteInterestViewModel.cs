using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public  class DeleteInterestViewModel
    {
      
        public List< int > TrucksIds { get; set; }
        public List<int > RegionsIds { get; set; }
        public int InterestId { get; set; }
    }
}
