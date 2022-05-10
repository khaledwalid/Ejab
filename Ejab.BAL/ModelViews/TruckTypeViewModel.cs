using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public class TruckTypeViewModel
    {
        public int TypeId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
           ErrorMessageResourceName = "TruckNameRequired")]   
        public string NameArb { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global),
         ErrorMessageResourceName = "TruckNameEngRequired")]
        public string Name { get; set; }
        public short FlgStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime? UpdatedOn { get; set; }
       public IEnumerable<TrucksViewModel > Trucks { get; set; }
    }
}
