using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace CDTKPMNC_STK_BE.DataAccess.DbConfigurations
{
   
        public class ConfigCampaign : IEntityTypeConfiguration<Campaign>
        {
            public void Configure(EntityTypeBuilder<Campaign> builder)
            {
                builder.Property(cp => cp.StartDate)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();
                builder.Property(cp => cp.EndDate)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();
            }
        }
    
}
