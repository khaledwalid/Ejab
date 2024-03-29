// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
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

    // MessageType
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class MessageType: BaseModel
    {
        public string Name { get; set; } // Name (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child Messages where [Message].[MessageTypeId] point to this entity (FK_Message_MessageType)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Message> Messages { get; set; } // Message.FK_Message_MessageType

        public MessageType()
        {
            Messages = new System.Collections.Generic.List<Message>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
