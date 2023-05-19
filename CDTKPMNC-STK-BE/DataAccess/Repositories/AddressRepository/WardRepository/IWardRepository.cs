using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository
{
    public interface IWardRepository : ICommonRepository<AddressWard>
    {
        List<AddressWard>? GetByDistrict(string districtId);
        List<AddressWard>? GetByDistrict(AddressProvince district);

        List<AddressWard>? GetByProvince(string provinceId);
        List<AddressWard>? GetByProvince(AddressProvince province);
    }
}
