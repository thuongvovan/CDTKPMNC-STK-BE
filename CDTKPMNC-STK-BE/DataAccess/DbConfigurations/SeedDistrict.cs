using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace CDTKPMNC_STK_BE.DataAccess.DbConfigurations
{
 
        public class SeedDistrict : IEntityTypeConfiguration<AddressDistrict>
        {
            public void Configure(EntityTypeBuilder<AddressDistrict> builder)
            {
                builder.HasData(
                    new AddressDistrict { Id = "760", Name = "1", FullName = "Quận 1", NameEN = "1", FullNameEN = "District 1", ProvinceId = "79" },
                    new AddressDistrict { Id = "761", Name = "12", FullName = "Quận 12", NameEN = "12", FullNameEN = "District 12", ProvinceId = "79" },
                    new AddressDistrict { Id = "764", Name = "Gò Vấp", FullName = "Quận Gò Vấp", NameEN = "Go Vap", FullNameEN = "Go Vap District", ProvinceId = "79" },
                    new AddressDistrict { Id = "765", Name = "Bình Thạnh", FullName = "Quận Bình Thạnh", NameEN = "Binh Thanh", FullNameEN = "Binh Thanh District", ProvinceId = "79" },
                    new AddressDistrict { Id = "766", Name = "Tân Bình", FullName = "Quận Tân Bình", NameEN = "Tan Binh", FullNameEN = "Tan Binh District", ProvinceId = "79" },
                    new AddressDistrict { Id = "767", Name = "Tân Phú", FullName = "Quận Tân Phú", NameEN = "Tan Phu", FullNameEN = "Tan Phu District", ProvinceId = "79" },
                    new AddressDistrict { Id = "768", Name = "Phú Nhuận", FullName = "Quận Phú Nhuận", NameEN = "Phu Nhuan", FullNameEN = "Phu Nhuan District", ProvinceId = "79" },
                    new AddressDistrict { Id = "769", Name = "Thủ Đức", FullName = "Thành phố Thủ Đức", NameEN = "Thu Duc", FullNameEN = "Thu Duc City", ProvinceId = "79" },
                    new AddressDistrict { Id = "770", Name = "3", FullName = "Quận 3", NameEN = "3", FullNameEN = "District 3", ProvinceId = "79" },
                    new AddressDistrict { Id = "771", Name = "10", FullName = "Quận 10", NameEN = "10", FullNameEN = "District 10", ProvinceId = "79" },
                    new AddressDistrict { Id = "772", Name = "11", FullName = "Quận 11", NameEN = "11", FullNameEN = "District 11", ProvinceId = "79" },
                    new AddressDistrict { Id = "773", Name = "4", FullName = "Quận 4", NameEN = "4", FullNameEN = "District 4", ProvinceId = "79" },
                    new AddressDistrict { Id = "774", Name = "5", FullName = "Quận 5", NameEN = "5", FullNameEN = "District 5", ProvinceId = "79" },
                    new AddressDistrict { Id = "775", Name = "6", FullName = "Quận 6", NameEN = "6", FullNameEN = "District 6", ProvinceId = "79" },
                    new AddressDistrict { Id = "776", Name = "8", FullName = "Quận 8", NameEN = "8", FullNameEN = "District 8", ProvinceId = "79" },
                    new AddressDistrict { Id = "777", Name = "Bình Tân", FullName = "Quận Bình Tân", NameEN = "Binh Tan", FullNameEN = "Binh Tan District", ProvinceId = "79" },
                    new AddressDistrict { Id = "778", Name = "7", FullName = "Quận 7", NameEN = "7", FullNameEN = "District 7", ProvinceId = "79" },
                    new AddressDistrict { Id = "783", Name = "Củ Chi", FullName = "Huyện Củ Chi", NameEN = "Cu Chi", FullNameEN = "Cu Chi District", ProvinceId = "79" },
                    new AddressDistrict { Id = "784", Name = "Hóc Môn", FullName = "Huyện Hóc Môn", NameEN = "Hoc Mon", FullNameEN = "Hoc Mon District", ProvinceId = "79" },
                    new AddressDistrict { Id = "785", Name = "Bình Chánh", FullName = "Huyện Bình Chánh", NameEN = "Binh Chanh", FullNameEN = "Binh Chanh District", ProvinceId = "79" },
                    new AddressDistrict { Id = "786", Name = "Nhà Bè", FullName = "Huyện Nhà Bè", NameEN = "Nha Be", FullNameEN = "Nha Be District", ProvinceId = "79" },
                    new AddressDistrict { Id = "787", Name = "Cần Giờ", FullName = "Huyện Cần Giờ", NameEN = "Can Gio", FullNameEN = "Can Gio District", ProvinceId = "79" }
                    );
            }
        }
    
}
