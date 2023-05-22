using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class CampaignRepository : CommonRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
