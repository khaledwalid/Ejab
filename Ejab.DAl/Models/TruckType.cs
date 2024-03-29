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


using System.ComponentModel.DataAnnotations;

namespace Ejab.DAl.Models
{

    // TruckType
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class TruckType : BaseModel
    {
        [Required]
        public string NameArb { get; set; } // Name (length: 50)
        [Required]
        public string Name { get; set; } // Name (length: 50)
                                         // Reverse navigation

        /// <summary>
        /// Child Offers where [Offer].[TruckTypeId] point to this entity (FK_dbo.Offer_dbo.TruckType_TruckTypeId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Offer> Offers { get; set; } // Offer.FK_dbo.Offer_dbo.TruckType_TruckTypeId
        /// <summary>
        /// Child Trucks where [Trucks].[TypeId] point to this entity (FK_dbo.Trucks_dbo.TruckType_TypeId)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Truck> Trucks { get; set; } // Trucks.FK_dbo.Trucks_dbo.TruckType_TypeId

        public TruckType()
        {
            Offers = new System.Collections.Generic.List<Offer>();
            Trucks = new System.Collections.Generic.List<Truck>();
        }
    }

}
// </auto-generated>
