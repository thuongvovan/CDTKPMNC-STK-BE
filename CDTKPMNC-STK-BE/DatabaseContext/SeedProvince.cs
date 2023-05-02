using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace CDTKPMNC_STK_BE.DatabaseContext
{
    namespace P02_FluentApi
    {
        public class SeedProvince : IEntityTypeConfiguration<AddressProvince>
        {
            public void Configure(EntityTypeBuilder<AddressProvince> builder)
            {
                builder.HasData( new AddressProvince { Id = "79", Name = "Hồ Chí Minh", FullName = "Thành phố Hồ Chí Minh", NameEN = "Ho Chi Minh", FullNameEN = "Ho Chi Minh City" } );
            }
        }
    }
}
