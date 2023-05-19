using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class AccountEndUserRepository : AccountRepository<AccountEndUser>, IAccountEndUserRepository
    {
        public AccountEndUserRepository(AppDbContext dbContext) : base(dbContext) { }

    }
}
