using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository
{
    public class WardRepository : CommonRepository<AddressWard>, IWardRepository
    {
        public WardRepository(AppDbContext dbContext) : base(dbContext) { }

        public List<AddressWard>? GetByDistrict(string districtId)
        {
            return _table.Where(w => w.DistrictId == districtId).ToList();
        }
        public List<AddressWard>? GetByDistrict(AddressProvince district)
        {
            return _table.Where(w => w.DistrictId == district.Id).ToList();
        }
        public List<AddressWard>? GetByProvince(string provinceId)
        {
            return _table.Where(w => w.ProvinceId == provinceId).ToList();
        }
        public List<AddressWard>? GetByProvince(AddressProvince province)
        {
            return _table.Where(w => w.ProvinceId == province.Id).ToList();
        }
    }
}
