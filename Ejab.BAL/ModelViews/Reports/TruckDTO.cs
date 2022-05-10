using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews.Reports
{
   public  class TruckDTO
    {
        public int Id { get; set; }
        public string  TruckNameArb { get; set; }
        public string TruckNameEng { get; set; }
        public string TruckParentNameArb { get; set; }
        public string TruckParentNameEng { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<TruckDTO> Childs { get; set; }
        public string  ParentImagePath { get; set; }
    }
}
