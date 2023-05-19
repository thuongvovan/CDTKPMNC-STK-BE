using CDTKPMNC_STK_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CDTKPMNC_STK_BE.DataAccess.DbConfigurations
{
 
        public class ConfigAccountEndUser : IEntityTypeConfiguration<AccountEndUser>
        {
            public void Configure(EntityTypeBuilder<AccountEndUser> builder)
            {
                builder.HasIndex(ua => ua.UserName)
                        .IsUnique();
            }
        }
    
}
