using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository
{
    public class DistrictRepository : CommonRepository<AddressDistrict>, IDistrictRepository
    {
        public DistrictRepository(AppDbContext dbContext) : base(dbContext) { }

        public List<AddressDistrict>? GetByProvince(string provinceId)
        {
            if (string.IsNullOrWhiteSpace(provinceId)) return null;
            var districts = _table.Where(d => d.ProvinceId == provinceId).ToList();
            if (districts.Count == 0) return null;
            return districts;           
        }

        public List<AddressDistrict>? GetByProvince(AddressProvince province)
        {
            if (province == null) return null;
            var districts =_table.Where(d => d.ProvinceId == province.Id).ToList();
            if (districts.Count == 0) return null;
            return districts;
        }

        public List<AddressWard>? GetWards(string districtId)
        {
            var district = GetById(districtId);
            if (district != null)
            {
                return district.Wards.ToList();
            }
            return null;
        }
    }
}
