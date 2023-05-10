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
        Store? GetById(Guid id);
        Store? GetByName(string name);
        void Delete(Store store);
        void DeleteById(Guid id);
        void Approve(Store store);
        void Reject(Store store);
        void Enable(Store store);
        void Disable(Store store);
        // void UpdateCompany(Store Store);

    }
}
