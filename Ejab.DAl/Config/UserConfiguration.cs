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


namespace Ejab.DAl
{

    // User
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class UserConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
    {
        public UserConfiguration()
            : this("dbo")
        {
        }

        public UserConfiguration(string schema)
        {
            ToTable("User", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Email).HasColumnName(@"Email").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.Mobile).HasColumnName(@"Mobile").HasColumnType("nvarchar").IsOptional().HasMaxLength(14);
            Property(x => x.ProfileImgPath).HasColumnName(@"ProfileImgPath").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.ProfileName).HasColumnName(@"ProfileName").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.RegisteredBy).HasColumnName(@"RegisteredBy").HasColumnType("int").IsRequired();
            Property(x => x.FaceBookId).HasColumnName(@"FaceBookId").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Address).HasColumnName(@"Address").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
            Property(x => x.CustomerType).HasColumnName(@"CustomerType").HasColumnType("int").IsRequired();
            Property(x => x.ResponsiblePerson).HasColumnName(@"ResponsiblePerson").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.IsAdmin).HasColumnName(@"IsAdmin").HasColumnType("bit").IsOptional();
            Property(x => x.OverAllrating).HasColumnName(@"OverAllrating").HasColumnType("decimal").IsOptional().HasPrecision(18,4);
            Property(x => x.AddressLatitude).HasColumnName(@"AddressLatitude").HasColumnType("decimal").IsOptional().HasPrecision(18,6);
            Property(x => x.AddressLongitude).HasColumnName(@"AddressLongitude").HasColumnType("decimal").IsOptional().HasPrecision(18,6);
            Property(x => x.Password).HasColumnName(@"Password").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.FlgStatus).HasColumnName(@"FlgStatus").HasColumnType("smallint").IsRequired();
            Property(x => x.CreatedBy).HasColumnName(@"CreatedBy").HasColumnType("int").IsRequired();
            Property(x => x.UpdatedBy).HasColumnName(@"UpdatedBy").HasColumnType("int").IsOptional();
            Property(x => x.CreatedOn).HasColumnName(@"CreatedOn").HasColumnType("datetime").IsRequired();
            Property(x => x.UpdatedOn).HasColumnName(@"UpdatedOn").HasColumnType("datetime").IsOptional();
        }
    }

}
// </auto-generated>
