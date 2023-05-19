using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository
{
    public interface IDistrictRepository : ICommonRepository<AddressDistrict>
    {
        List<AddressDistrict>? GetByProvince(string provinceId);
        List<AddressDistrict>? GetByProvince(AddressProvince province);
        List<AddressWard>? GetWards(string districtId);
    }
}
