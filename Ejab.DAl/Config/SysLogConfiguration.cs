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

    // SysLogs
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class SysLogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SysLog>
    {
        public SysLogConfiguration()
            : this("dbo")
        {
        }

        public SysLogConfiguration(string schema)
        {
            ToTable("SysLogs", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ActionId).HasColumnName(@"ActionId").HasColumnType("int").IsOptional();
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.CreatedBy).HasColumnName(@"CreatedBy").HasColumnType("int").IsOptional();
            Property(x => x.CreatedOn).HasColumnName(@"CreatedOn").HasColumnType("datetime").IsOptional();
            Property(x => x.FlgStatus).HasColumnName(@"FlgStatus").HasColumnType("smallint").IsOptional();
            Property(x => x.PredefinedActionId).HasColumnName(@"PredefinedAction_Id").HasColumnType("int").IsOptional();

            // Foreign keys
            HasOptional(a => a.PredefinedAction).WithMany(b => b.SysLogs).HasForeignKey(c => c.PredefinedActionId).WillCascadeOnDelete(false); // FK_dbo.SysLogs_dbo.PredefinedActions_PredefinedAction_Id
        }
    }

}
// </auto-generated>