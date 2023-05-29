using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.CampaignEndUsersRepository
{
    public class CampaignEndUsersRepository : CommonRepository<CampaignEndUsers>, ICampaignEndUsersRepository
    {
        public CampaignEndUsersRepository(AppDbContext context) : base(context)
        {
        }
    }
}
