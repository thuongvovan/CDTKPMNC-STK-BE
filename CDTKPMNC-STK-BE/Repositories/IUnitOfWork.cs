namespace CDTKPMNC_STK_BE.DataAccess
{
    public interface IUnitOfWork
    {
        IAccountEndUserRepository EndUserAccountRepository { get; }
        IAccountAdminRepository AdminAccountRepository { get; }
        void Commit();
    }
}
