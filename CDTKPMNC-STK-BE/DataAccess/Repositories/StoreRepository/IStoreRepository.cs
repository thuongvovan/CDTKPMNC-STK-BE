using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface IStoreRepository : ICommonRepository<Store>
    {
        List<Store>? GetApproved();
        List<Store>? GetRejected();
        List<Store>? GetNeedApproval();
        Store? GetByName(string name);
        void Approve(Store store);
        void Reject(Store store);
        void Enable(Store store);
        void Disable(Store store);
        // void UpdateCompany(Store Store);

    }
}
