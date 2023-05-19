using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CDTKPMNC_STK_BE.DataAccess.DbConfigurations
{
    public class ConfigAccount : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.UseTpcMappingStrategy();
            builder.Property(acc => acc.DateOfBirth)
                .HasConversion<DateOnlyConverter, DateOnlyComparer>();
        }
    }
}
