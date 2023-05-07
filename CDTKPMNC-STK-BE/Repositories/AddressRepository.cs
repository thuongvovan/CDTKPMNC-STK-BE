using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _dbContext;
        public AddressRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<AddressProvince> GetAllProvines()
        {
            return _dbContext.Provinces.ToList();
        }
        public AddressProvince? GetProvineById(string provinceId)
        {
            return _dbContext.Provinces!.Find(provinceId);
        }
        
        public List<AddressDistrict> GetDistrictsByProvineId(string provinceId)
        {
            AddressProvince? province = _dbContext.Provinces.Find(provinceId);
            if (province == null)
            {
                return new List<AddressDistrict>();
            }
            return province.Districts.ToList();
        }

        public AddressDistrict? GetDistrictById(string districtId)
        {
            return _dbContext.Districts.Find(districtId);
        }

        public AddressWard? GetWardById(string wardId)
        {
            return _dbContext.Wards!.Find(wardId);
        }

        public List<AddressWard> GetWardsByDistrictId(string districtId)
        {
            AddressDistrict? district = _dbContext.Districts.Find(districtId);
            if (district == null)
            {
                return new List<AddressWard>();
            }
            return district.Wards.ToList();
        }
    }
}
