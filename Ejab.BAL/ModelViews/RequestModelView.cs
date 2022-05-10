using Ejab.DAl.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.ModelViews
{
   public  class RequestModelView
    {
        public int Id { get; set; }
        [Display(Name = "RequestNumber", ResourceType = typeof(Resources.Global))]
        public int RequestNumber { get; set; }
        public int UserId { get; set; }
        public UserViewModel   Requster { get; set; }
        [Required(ErrorMessage = "031")]
        [Display(Name = "Requestdate", ResourceType = typeof(Resources.Global))]
        public System.DateTime Requestdate { get; set; } // Requestdate
        [Required(ErrorMessage = "041")]
        [Display(Name = "Title", ResourceType = typeof(Resources.Global))]
        public string Title { get; set; } // Title (length: 100)
        [Required(ErrorMessage = "042")]
        [Display(Name = "Description", ResourceType = typeof(Resources.Global))]
        public string Description { get; set; } // Description (length: 500)
        [Display(Name = "AddressLongitude", ResourceType = typeof(Resources.Global))]
        public decimal? LocationFromlongitude { get; set; } // LocationFromlongitude
        [Display(Name = "AdressLatitude", ResourceType = typeof(Resources.Global))]
        public decimal? LocationFromLatitude { get; set; } // locationFromLatitude
        [Display(Name = "LocationFrom", ResourceType = typeof(Resources.Global))]
        public string LocationFrom { get; set; } // LocationFrom (length: 250)
        [Display(Name = "AddressLongitude", ResourceType = typeof(Resources.Global))]
        public decimal? LocationToLongitude { get; set; } // LocationToLongitude
        [Display(Name = "AdressLatitude", ResourceType = typeof(Resources.Global))]
        public decimal? LocationToLatitude { get; set; } // LocationToLatitude
        [Required(ErrorMessage = "045")]
        [Display(Name = "LocationTo", ResourceType = typeof(Resources.Global))]
        public string LocationTo { get; set; } // LocationTo (length: 250)
        [Display(Name = "RequestState", ResourceType = typeof(Resources.Global))]
        public RequestStates RequestState { get; set; } // State
        [Display(Name = "IsActive", ResourceType = typeof(Resources.Global))]
        public bool IsActive { get; set; } // IsActive
        [DataType(DataType.Date)]
        [Display(Name = "ExpireDate", ResourceType = typeof(Resources.Global))]
        public System.DateTime ExpireDate { get; set; } // ExpireDate
        [Display(Name = "Rating", ResourceType = typeof(Resources.Global))]
        public IQueryable <ServiceProviderRatingViewModel > Rating { get; set; }
        [Display(Name = "requestDetails", ResourceType = typeof(Resources.Global))]
        public IEnumerable<RequestDetailsModelView> requestDetails { get; set; }
        [Display(Name = "ProposalPrice", ResourceType = typeof(Resources.Global))]
        public IQueryable<ProposalPriceModelView> ProposalPrice { get; set; }
        [Display(Name = "StartingDate", ResourceType = typeof(Resources.Global))]
        public System.DateTime StartingDate { get; set; }
        [Display(Name = "Period", ResourceType = typeof(Resources.Global))]
        public string Period { get; set; }
        [MaxLength(400)]
        [Display(Name = "quantity", ResourceType = typeof(Resources.Global))]
        public string Quantity { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "ItemsInfo", ResourceType = typeof(Resources.Global))]
        public string ItemsInfo { get; set; }
        [Display(Name = "Region", ResourceType = typeof(Resources.Global))]
        public RegionModelView  Region { get; set; }
        public string RegionName { get; set; }
        [DataType(DataType.MultilineText )]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Global))]
        public string Notes { get; set; }
        [Display(Name = "RequestType", ResourceType = typeof(Resources.Global))]
        public RequestType RequestType { get; set; }
        [Display(Name = "PermissionDate", ResourceType = typeof(Resources.Global))]
        public DateTime PermissionDate { get; set; }
        public int Count { get; set; }
        public bool  IsAccepted { get; set; }
        public int RegionId { get; set; }




    }
}
