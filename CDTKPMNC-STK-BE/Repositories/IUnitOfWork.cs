using CDTKPMNC_STK_BE.Repositories;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IUnitOfWork
    {
        IAccountEndUserRepository AccountEndUserRepo { get; }
        IAccountAdminRepository AccountAdminRepo { get; }
        IAccountPartnerRepository AccountPartnerRepo { get; }
        IAddressRepository AddressRepo { get; }
        ICompanyRepository CompanyRepo { get; }
        IStoreRepository StoreRepo { get; }
        IGameRepository GameRepo { get; }
        IProductCategoryRepository ProductCategoryRepo { get; }
        void Commit();
    }
}
