using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository
{
    public interface IProvinceRepository : ICommonRepository<AddressProvince>
    {
        List<AddressDistrict>? GetDistricts(string proviceId);
        List<AddressWard>? GetWards(string proviceId);
    }
}
