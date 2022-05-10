using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class RegionModelView
    {
        public int Id { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
             ErrorMessageResourceName = "RegionNamereuired")]
        public string Name { get; set; } // Name (length: 50) 
        public int? ParanetId { get; set; }
        public string ParaneName { get; set; }
         public IEnumerable<RegionModelView> ChildModel { get; set; }
        public IEnumerable<RegionModelView> ParentNodes { get; set; }
        public short FlgStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime? UpdatedOn { get; set; }
        public string NameArb { get; set; }
    }
}
