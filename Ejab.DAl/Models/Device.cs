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


namespace Ejab.DAl.Models
{

    // Devices
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Device : BaseModel
    {
        public string SerialNumber { get; set; } // SerialNumber (length: 200)
        public string DeviceToken { get; set; } // DeviceToken (length: 500)
        public string DeviceType { get; set; } // DeviceType (length: 100)
        public int? UserDeviceId { get; set; } // UserDevice_Id

        // Foreign keys

        /// <summary>
        /// Parent UserDevice pointed by [Devices].([UserDeviceId]) (FK_dbo.Devices_dbo.UserDevices_UserDevice_Id)
        /// </summary>
        public virtual UserDevice UserDevice { get; set; } // FK_dbo.Devices_dbo.UserDevices_UserDevice_Id
    }

}
// </auto-generated>
