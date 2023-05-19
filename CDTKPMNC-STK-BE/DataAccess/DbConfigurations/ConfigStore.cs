using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace CDTKPMNC_STK_BE.DataAccess.DbConfigurations
{
   
        public class ConfigStore : IEntityTypeConfiguration<Store>
        {
            public void Configure(EntityTypeBuilder<Store> builder)
            {
                builder.Property(s => s.OpenTime)
                    .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
                builder.Property(s => s.CloseTime)
                    .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
            }
        }
    
}
