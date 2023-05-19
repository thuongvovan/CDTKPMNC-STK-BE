using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface IAccountRepository<TAccount> : ICommonRepository<TAccount> where TAccount : Account
    {
        TAccount? GetByUserName(string account);
    }
}
