using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository
{
    public class ProvinceRepository : CommonRepository<AddressProvince>, IProvinceRepository
    {
        public ProvinceRepository(AppDbContext dbContext) : base(dbContext) { }
        public List<AddressDistrict>? GetDistricts(string proviceId)
        {
            var provice = GetById(proviceId);
            if (provice != null)
            {
                return provice.Districts.ToList();
            }
            return null;
        }
        public List<AddressWard>? GetWards(string proviceId)
        {
            var provice = GetById(proviceId);
            if (provice != null)
            {
                return provice.Wards.ToList();
            }
            return null;
        }
    }
}
