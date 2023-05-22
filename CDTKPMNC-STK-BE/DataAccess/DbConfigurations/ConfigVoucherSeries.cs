using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace CDTKPMNC_STK_BE.DataAccess.DbConfigurations
{
   
        public class ConfigVoucherSeries : IEntityTypeConfiguration<VoucherSeries>
        {
            public void Configure(EntityTypeBuilder<VoucherSeries> builder)
            {
                //builder.Property(vc => vc.ExpiresOn)
                //    .HasConversion<DateOnlyConverter, DateOnlyComparer>();
            }
        }

        public class ConfigVoucherSeriesCampaigns : IEntityTypeConfiguration<CampaignVoucherSeries>
        {
            public void Configure(EntityTypeBuilder<CampaignVoucherSeries> builder)
            {
                builder.Property(vsc => vsc.ExpiresOn)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();
            }
    }

}
