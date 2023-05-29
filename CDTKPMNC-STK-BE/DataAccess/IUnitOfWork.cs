using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository;
using CDTKPMNC_STK_BE.DataAccess.Repositories.CampaignEndUsersRepository;
using CDTKPMNC_STK_BE.DataAccess.Repositories.VoucherSeriesRepository;
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
        ICampaignRepository CampaignRepo { get; }
        ICampaignVoucherSeriesRepository CampaignVoucherSeriesRepo { get; }
        ICampaignEndUsersRepository CampaignEndUsersRepo { get; }
        IVoucherRepository VoucherRepo { get; }
        IVoucherSeriesRepository VoucherSeriesRepo { get; }
        IProductCategoryRepository ProductCategoryRepo { get; }
        IProductItemRepository ProductItemRepo { get; }
        INoticationRepository NoticationRepo { get; }
        void Commit();
    }
}
