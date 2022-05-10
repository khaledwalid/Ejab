using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.DAl.Config
{
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.32.0.0")]
    public partial class DevicesConfig : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Devices>
    {
        public DevicesConfig()
            : this("dbo")
        {
        }

        public DevicesConfig(string schema)
        {
            ToTable("Devices", schema);
            HasKey(x => x.Id);

            Property(x => x.SerialNumber).HasColumnName(@"SerialNumber").HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.DeviceToken).HasColumnName(@"DeviceToken").HasColumnType("nvarchar").HasMaxLength(500);
            Property(x => x.DeviceType).HasColumnName(@"DeviceType").HasColumnType("nvarchar").HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }
}
