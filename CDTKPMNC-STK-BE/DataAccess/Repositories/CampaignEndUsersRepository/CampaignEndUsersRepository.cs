using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.CampaignEndUsersRepository
{
    public class CampaignEndUsersRepository : CommonRepository<CampaignEndUsers>, ICampaignEndUsersRepository
    {
        public CampaignEndUsersRepository(AppDbContext context) : base(context)
        {
            
        }

        public List<CampaignEndUsers> GetByUserWinCampaign(Campaign campaign, AccountEndUser accountEndUser)
        {
            return _table.Where(cu => cu.EndUserId == accountEndUser.Id && cu.CampaignId == campaign.Id && (cu.IsWinner ?? false)).ToList();
        }

        public List<CampaignEndUsers> GetByUserCampaign(Campaign campaign, AccountEndUser accountEndUser)
        {
            return _table.Where(cu => cu.EndUserId == accountEndUser.Id && cu.CampaignId == campaign.Id).ToList();
        }
    }
}
