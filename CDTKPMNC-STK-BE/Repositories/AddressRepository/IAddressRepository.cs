using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IAddressRepository
    {
        List<AddressProvince> GetAllProvines();
        AddressProvince? GetProvineById(string provinceId);
        List<AddressDistrict> GetDistrictsByProvineId(string provinceId);
        AddressDistrict? GetDistrictById(string districtId);
        List<AddressWard> GetWardsByDistrictId(string districtId);
        AddressWard? GetWardById(string wardId);
    }
}
