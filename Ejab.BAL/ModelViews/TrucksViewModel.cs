using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
  public class TrucksViewModel
    {
       public int Id { get; set; }
        [Required(ErrorMessage ="048 ")]
        public string Name { get; set; } // Name (length: 50)       
        public string NameArb { get; set; } // Name (length: 50)
        public int TruckTypeId { get; set; }
        public string  TruckTypeName { get; set; }
        public string TruckTypeNameArb { get; set; }
        public   TruckTypeViewModel  truckType { get; set; }
        public int? Weight { get; set; } // Weight
         public bool? IsOcuppied { get; set; } // IsOcuppied
        public int? AvialableNo { get; set; } // AvialableNo
        public decimal? Capacity { get; set; } // Capacity
        public string Description { get; set; } // Description (length: 250)
        public int? Width { get; set; } // Width
        public int? Height { get; set; } // height
        public int? ParenetId { get; set; }
        public string  ParenetName{ get; set; }
        public string ParenetNameArb { get; set; }
        // public TrucksViewModel paraent { get; set; }
        public IEnumerable<TrucksViewModel> ChildModel { get; set; }
        public IEnumerable<TrucksViewModel> ParentNodes { get; set; }
        public short FlgStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime? UpdatedOn { get; set; }
        public string TruckImagePath { get; set; }



    }
}
