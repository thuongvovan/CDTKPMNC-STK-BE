using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IStoreRepository
    {
        void Add(Store store);
        List<Store> GetAll();
        List<Store> GetApproved();
        List<Store> GetRejected();
        List<Store> GetNeedApproval();
        Store? GetStoreById(Guid id);
        Store? GetStoreByName(string name);
        void DeleteStore(Store store);
        void DeleteStoreById(Guid id);
        void ApproveStore(Store store);
        void RejectStore(Store store);
        void EnableStore(Store store);
        void DisableStore(Store store);
        // void UpdateCompany(Store Store);

    }
}
