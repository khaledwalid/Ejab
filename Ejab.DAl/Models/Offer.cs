// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.5
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


using Ejab.DAl.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ejab.DAl.Models
{

    // Offer
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Offer: BaseModel
    {
        public long OfferNumber { get; set; } // OfferNumber
        public System.DateTime OfferDate { get; set; } // OfferDate
        public System.DateTime PublishDate { get; set; } // PublishDate
        public string Title { get; set; } // Title (length: 250)
        public string Description { get; set; } // Description (length: 500)
        public string Address { get; set; } // Address (length: 250)
        public decimal? AdressLatitude { get; set; } // AdressLatitude
        public decimal? AddressLongitude { get; set; } // AddressLongitude
        public decimal Price { get; set; } // Price
        public string  Quantity { get; set; } // quantity
        public string  Period { get; set; }
        public bool? IsDiscount { get; set; } // IsDiscount
        public double? DiscountPecent { get; set; } // DiscountPecent
        public decimal? DiscountAmount { get; set; } // DiscountAmount
        public bool? IsActive { get; set; } // IsActive
        public int? UserId { get; set; } // UserId
        public System.DateTime? ExpireDate { get; set; } // ExpireDate
        public int? MaxCustomerNumbers { get; set; } // MaxCustomerNumbers
        public System.DateTime? MaxExpireDate { get; set; } // MaxExpireDate
        public int TruckTypeId { get; set; } // TruckTypeId
        public string ImageUrl { get; set; } // ImageUrl
        public int? RegionId { get; set; }
        public string Notes { get; set; }
        //[ForeignKey("ServiceTypeId")]
        //public virtual ServiceType ServiceType { get; set; }
        //public int? ServiceTypeId { get; set; }
       

        // Reverse navigation

        /// <summary>
        /// Child AcceptOffers where [AcceptOffers].[OfferId] point to this entity (FK_dbo.AcceptOffers_dbo.Offer_OfferId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<AcceptOffer> AcceptOffers { get; set; } // AcceptOffers.FK_dbo.AcceptOffers_dbo.Offer_OfferId
        /// <summary>
        /// Child OfferDetails where [OfferDetails].[OfferId] point to this entity (FK_dbo.OfferDetails_dbo.Offer_OfferId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<OfferDetail> OfferDetails { get; set; } // OfferDetails.FK_dbo.OfferDetails_dbo.Offer_OfferId
        /// <summary>
        /// Child OfferImages where [OfferImages].[OfferId] point to this entity (FK_dbo.OfferImages_dbo.Offer_OfferId)
        /// </summary>
     //   public virtual System.Collections.Generic.ICollection<OfferImage> OfferImages { get; set; } // OfferImages.FK_dbo.OfferImages_dbo.Offer_OfferId
        /// <summary>
        /// Child ServiceTypes (Many-to-Many) mapped by table [ServiceTypeOffers]
        /// </summary>
        //public virtual System.Collections.Generic.ICollection<ServiceType> ServiceTypes { get; set; } // Many to many mapping

        // Foreign keys

        /// <summary>
        /// Parent TruckType pointed by [Offer].([TruckTypeId]) (FK_dbo.Offer_dbo.TruckType_TruckTypeId)
        /// </summary>
        public virtual TruckType TruckType { get; set; } // FK_dbo.Offer_dbo.TruckType_TruckTypeId

        /// <summary>
        /// Parent User pointed by [Offer].([UserId]) (FK_dbo.Offer_dbo.User_UserId)
        /// </summary>
        public virtual User User { get; set; } // FK_dbo.Offer_dbo.User_UserId
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }
        public virtual System.Collections.Generic.ICollection<Message> Messages { get; set; }

        public Offer()
        {
            AcceptOffers = new System.Collections.Generic.List<AcceptOffer>();
            OfferDetails = new System.Collections.Generic.List<OfferDetail>();
            //  OfferImages = new System.Collections.Generic.List<OfferImage>();
            //ServiceTypes = new System.Collections.Generic.List<ServiceType>();
            Messages = new HashSet<Message>();
        }
    }

}
// </auto-generated>
