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

    // SuggestionsComplaint
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class SuggestionsComplaintConfig : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SuggestionsComplaint>
    {
        public SuggestionsComplaintConfig()
            : this("dbo")
        {
        }

        public SuggestionsComplaintConfig(string schema)
        {
            ToTable("SuggestionsComplaint", schema);
            HasKey(x => x.Id);

            Property(x => x.Date).HasColumnName(@"Date").HasColumnType("date").IsRequired();
            Property(x => x.Cause).HasColumnName(@"Cause").HasColumnType("nvarchar").IsRequired().HasMaxLength(150);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            Property(x => x.CustomerId).HasColumnName(@"CustomerId").HasColumnType("int").IsRequired();
            Property(x => x.Admin).HasColumnName(@"Admin").HasColumnType("int").IsOptional();
            Property(x => x.ComplainUserId).HasColumnName(@"ComplainUserId").HasColumnType("int").IsRequired();

            // Foreign keys
            HasOptional(a => a.User_Admin).WithMany(b => b.Admin).HasForeignKey(c => c.Admin).WillCascadeOnDelete(false); // FK_SuggestionsComplaint_User1
            HasRequired(a => a.ComplainUser).WithMany(b => b.ComplainUser).HasForeignKey(c => c.ComplainUserId).WillCascadeOnDelete(false); // FK_SuggestionsComplaint_User2
            HasRequired(a => a.Customer).WithMany(b => b.Customer).HasForeignKey(c => c.CustomerId).WillCascadeOnDelete(false); // FK_SuggestionsComplaint_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
