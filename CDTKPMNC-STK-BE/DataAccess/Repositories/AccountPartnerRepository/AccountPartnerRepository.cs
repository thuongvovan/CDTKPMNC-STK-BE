using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class AccountPartnerRepository : AccountRepository<AccountPartner>, IAccountPartnerRepository
    {
        public AccountPartnerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
