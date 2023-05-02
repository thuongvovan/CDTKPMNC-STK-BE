using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace CDTKPMNC_STK_BE.DatabaseContext
{
    namespace P02_FluentApi
    {
        public class ConfigVoucherSeries : IEntityTypeConfiguration<VoucherSeries>
        {
            public void Configure(EntityTypeBuilder<VoucherSeries> builder)
            {
                builder.Property(vc => vc.ExpiresOn)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();
            }
        }
    }
}
