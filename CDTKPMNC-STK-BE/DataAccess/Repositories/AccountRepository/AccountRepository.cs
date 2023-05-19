using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class AccountRepository<TAccount> : CommonRepository<TAccount>, IAccountRepository<TAccount> where TAccount : Account
    {
        public AccountRepository(AppDbContext dbContext) : base(dbContext) { }
        public TAccount? GetByUserName(string account)
        {
            return _table.SingleOrDefault(a => a.UserName == account.ToLower());
        }
    }
}
