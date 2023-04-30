namespace CDTKPMNC_STK_BE.DataAccess
{
    public interface IUnitOfWork
    {
        IEndUserAccountRepository EndUserAccountRepository { get; }
        void Commit();
    }
}
