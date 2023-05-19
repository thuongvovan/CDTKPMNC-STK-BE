using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class AccountAdminRepository : AccountRepository<AccountAdmin>, IAccountAdminRepository
    {
        public AccountAdminRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
