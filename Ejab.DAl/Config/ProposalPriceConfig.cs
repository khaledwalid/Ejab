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

    // proposalPrice
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class ProposalPriceConfig : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProposalPrice>
    {
        public ProposalPriceConfig()
            : this("dbo")
        {
        }

        public ProposalPriceConfig(string schema)
        {
            ToTable("proposalPrice", schema);
            HasKey(x => x.Id);

            Property(x => x.ReqestId).HasColumnName(@"ReqestId").HasColumnType("int").IsRequired();
            Property(x => x.Date).HasColumnName(@"Date").HasColumnType("datetime").IsRequired();
            Property(x => x.Price ).HasColumnName(@"price").HasColumnType("money").IsRequired().HasPrecision(19,4);
         

            // Foreign keys
          ////  HasRequired(a => a.RequestDetaile).WithMany(b => b.ProposalPrices).HasForeignKey(c => c.RequestDetaileId).WillCascadeOnDelete(false); // FK_proposalPrice_Request
          //  HasRequired(a => a.User).WithMany(b => b.ProposalPrices).HasForeignKey(c => c.ServiceProviderId).WillCascadeOnDelete(false); // FK_proposalPrice_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
