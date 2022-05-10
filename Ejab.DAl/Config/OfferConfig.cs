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

    // Offer
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class OfferConfig : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Offer>
    {
        public OfferConfig()
            : this("dbo")
        {
        }

        public OfferConfig(string schema)
        {
            ToTable("Offer", schema);
            HasKey(x => x.Id);

            Property(x => x.OfferNumber).HasColumnName(@"OfferNumber").HasColumnType("bigint").IsRequired();
            Property(x => x.OfferDate).HasColumnName(@"OfferDate").HasColumnType("datetime").IsRequired();
            Property(x => x.PublishDate).HasColumnName(@"PublishDate").HasColumnType("datetime").IsRequired();
            Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            Property(x => x.Address).HasColumnName(@"Address").HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.AdressLatitude).HasColumnName(@"AdressLatitude").HasColumnType("decimal").IsOptional().HasPrecision(18, 6);
            Property(x => x.AddressLongitude).HasColumnName(@"AddressLongitude").HasColumnType("decimal").IsOptional().HasPrecision(18, 6);
            Property(x => x.Price).HasColumnName(@"Price").HasColumnType("money").IsRequired().HasPrecision(19, 4);
            Property(x => x.IsDiscount).HasColumnName(@"IsDiscount").HasColumnType("bit").IsOptional();
            Property(x => x.DiscountPecent).HasColumnName(@"DiscountPecent").HasColumnType("float").IsOptional();
            Property(x => x.DiscountAmount).HasColumnName(@"DiscountAmount").HasColumnType("money").IsOptional().HasPrecision(19, 4);
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsOptional();
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int").IsOptional();
            Property(x => x.ExpireDate).HasColumnName(@"ExpireDate").HasColumnType("date").IsOptional();
            Property(x => x.MaxCustomerNumbers).HasColumnName(@"MaxCustomerNumbers").HasColumnType("int").IsOptional();
            Property(x => x.MaxExpireDate).HasColumnName(@"MaxExpireDate").HasColumnType("date").IsOptional();
            // Foreign keys
            HasOptional(a => a.User).WithMany(b => b.Offers).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_Offer_User
            //this.HasMany(c => c.ServiceTypes )
            //  .WithMany()
            //  .Map(m => m.ToTable("OfferServiceTypes"));
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>