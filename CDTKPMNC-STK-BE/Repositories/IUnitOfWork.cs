namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IUnitOfWork
    {
        IAccountEndUserRepository AccountEndUserRepository { get; }
        IAccountAdminRepository AccountAdminRepository { get; }
        IAccountPartnerRepository AccountPartnerRepository { get; }
        IAddressRepository AddressRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IStoreRepository StoreRepository { get; }
        void Commit();
    }
}
