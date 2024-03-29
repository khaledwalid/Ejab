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


using Ejab.DAl.Models;

namespace Ejab.DAl
{

    // UserDevices
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class UserDevice : BaseModel
    {
        public int UserId { get; set; } // UserId
        public int DeviceId { get; set; } // DeviceId
        // Reverse navigation

        /// <summary>
        /// Child Devices where [Devices].[UserDevice_Id] point to this entity (FK_dbo.Devices_dbo.UserDevices_UserDevice_Id)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Device> Devices { get; set; } // Devices.FK_dbo.Devices_dbo.UserDevices_UserDevice_Id

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [UserDevices].([UserId]) (FK_dbo.UserDevices_dbo.User_UserId)
        /// </summary>
        public virtual User User { get; set; } // FK_dbo.UserDevices_dbo.User_UserId

        public UserDevice()
        {
            Devices = new System.Collections.Generic.List<Device>();
        }
    }

}
// </auto-generated>
