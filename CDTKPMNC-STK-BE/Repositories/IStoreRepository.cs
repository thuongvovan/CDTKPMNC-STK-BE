using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IStoreRepository
    {
        void AddStore(Store store);
        List<Store> GetAllCompanys();
        Store? GetStoreById(Guid id);
        Store? GetStoreByName(string name);
        void DeleteStore(Store store);
        void DeleteStoreById(Guid id);
        void ActiveStore(Store store);
        void DeactiveStore(Store store);
        void EnableStore(Store store);
        void DisableStore(Store store);
        // void UpdateCompany(Store Store);

    }
}
