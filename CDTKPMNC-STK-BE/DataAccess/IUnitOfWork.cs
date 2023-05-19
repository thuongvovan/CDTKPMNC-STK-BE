using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public interface IUnitOfWork
    {
        IAccountRepository<Account> AccountRepo { get; }
        IAccountEndUserRepository AccountEndUserRepo { get; }
        IAccountAdminRepository AccountAdminRepo { get; }
        IAccountPartnerRepository AccountPartnerRepo { get; }
        IProvinceRepository ProvinceRepo { get; }
        IDistrictRepository DistrictRepo { get; }
        IWardRepository WardRepo { get; }
        ICompanyRepository CompanyRepo { get; }
        IStoreRepository StoreRepo { get; }
        IGameRepository GameRepo { get; }
        IProductCategoryRepository ProductCategoryRepo { get; }
        void Commit();
    }
}
