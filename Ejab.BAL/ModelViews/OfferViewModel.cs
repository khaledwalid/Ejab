using Ejab.DAl.Common;
using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ejab.BAL.ModelViews
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "030")]
   //  [Display(Name = "OfferNumber", ResourceType = typeof(Resources.Global.OfferNumber))]
        public long OfferNumber { get; set; } // OfferNumber
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        [Required(ErrorMessage = "031")]
        public System.DateTime OfferDate { get; set; } // OfferDate
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        [Required(ErrorMessage = "032")]
        public System.DateTime PublishDate { get; set; } // PublishDate
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "033")]
        public string Title { get; set; } // Title (length: 250)
        [Required(ErrorMessage = "034")]
        public string Description { get; set; } // Description (length: 500)
        public string Address { get; set; } // Address (length: 250)
        public decimal? AdressLatitude { get; set; } // AdressLatitude
        public decimal? AddressLongitude { get; set; } // AddressLongitude
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "035")]
        public decimal Price { get; set; } // Price
      
        public string  quantity { get; set; }
        public string Period { get; set; }
        public bool? IsDiscount { get; set; } // IsDiscount
        public double? DiscountPecent { get; set; } // DiscountPecent
        public decimal? DiscountAmount { get; set; } // DiscountAmount
        public bool? IsActive { get; set; } // IsActive
        public int? ServiceProviderUserId { get; set; } // UserId
        public UserViewModel ServiceProvider { get; set; }
        public string ServiceProvidername { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal ServiceProviderRating { get; set; }
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public System.DateTime ExpireDate { get; set; } // ExpireDate        
        public int? MaxCustomerNumbers { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime? MaxExpireDate { get; set; }
        //    public IEnumerable<OfferImagesViewModel> OfferImages { get; set; }
        public int  ServiceTypeId { get; set; }
        public ServiceTypeViewModel ServiceType { get; set; }
        public IEnumerable<OfferDetailViewModel> OfferDetails { get; set; }
        public int TruckTypeId { get; set; }
        public string TruckTypeName { get; set; }
        public string TruckTypeNameArb { get; set; }
        public TruckTypeViewModel TruckType { get; set; }
        public int Count { get; set; }
        public IEnumerable<AcceptofferViewModel> AcceptedBy { get; set; }
        public RegionModelView  Region { get; set; }
        public int? RegionId { get; set; }
        public string  RegionName { get; set; }
        public string Notes { get; set; }
        public bool? Accepted { get; set; }
        public OfferState State { get; set; }
        public IEnumerable<AcceptUserviewModel> AcceptedUsers { get; set; }
        public int FlgStatus { get; set; }

    }
}
