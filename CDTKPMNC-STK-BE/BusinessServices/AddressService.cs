using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class AddressService : CommonService
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardRepository _wardRepository;
        public AddressService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _provinceRepository = _unitOfWork.ProvinceRepo;
            _districtRepository = unitOfWork.DistrictRepo;
            _wardRepository = unitOfWork.WardRepo;
        }

        public AddressProvince? GetProvinceById(string provinceId)
        {
            return _provinceRepository.GetById(provinceId);
        }

        public AddressDistrict? GetDistrictById(string districtId)
        {
            return _districtRepository.GetById(districtId);
        }

        public AddressWard? GetWardById(string wardId)
        {
            return _wardRepository.GetById(wardId);
        }

        public List<AddressProvince> GetAllProvines()
        {
            return _provinceRepository.GetAll().ToList();
        }

        public List<AddressDistrict> GetDistrictsByProvineId(string provineId)
        {
            var districts = _districtRepository.GetByProvince(provineId)!;
            if (districts == null)
            {
                return new List<AddressDistrict>(0);
            }
            return districts.ToList();
        }

        public List<AddressWard> GetWardsByDistrictId(string districtId)
        {
            var wards = _wardRepository.GetByDistrict(districtId)!;
            return wards.ToList();
        }

        public List<AddressWard> GetWardsByDistrict(AddressDistrict district)
        {
            var wards = district.Wards;
            if (wards == null)
            {
                return new List<AddressWard>(0);
            }
            return wards.ToList();
        }
    }
}
