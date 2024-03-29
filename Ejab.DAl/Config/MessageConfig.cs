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


namespace Ejab.DAl
{

    // Message
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class MessageConfig : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Message>
    {
        public MessageConfig()
            : this("dbo")
        {
        }

        public MessageConfig(string schema)
        {
            ToTable("Message", schema);
            HasKey(x => x.Id);

            Property(x => x.Date).HasColumnName(@"Date").HasColumnType("datetime").IsRequired();
            Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            Property(x => x.Status).HasColumnName(@"Status").HasColumnType("bit").IsOptional();
            Property(x => x.SenderId).HasColumnName(@"SenderId").HasColumnType("int").IsRequired();
            Property(x => x.ReciverId).HasColumnName(@"ReciverId").HasColumnType("int").IsRequired();
            Property(x => x.RequestId).HasColumnName(@"RequestId").HasColumnType("int").IsRequired();
           Property(x => x.MessageType).HasColumnName(@"MessageType").HasColumnType("int").IsOptional();

            // Foreign keys
            HasRequired(a => a.Reciver).WithMany(b => b.Reciver).HasForeignKey(c => c.ReciverId).WillCascadeOnDelete(false); // FK_Message_User1
            HasRequired(a => a.Request).WithMany(b => b.Messages).HasForeignKey(c => c.RequestId).WillCascadeOnDelete(false); // FK_Message_Request
            HasRequired(a => a.Sender).WithMany(b => b.Sender).HasForeignKey(c => c.SenderId).WillCascadeOnDelete(false); // FK_Message_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
