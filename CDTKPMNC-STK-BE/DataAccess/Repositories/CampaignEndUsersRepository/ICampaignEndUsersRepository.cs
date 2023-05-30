using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.CampaignEndUsersRepository
{
    public interface ICampaignEndUsersRepository : ICommonRepository<CampaignEndUsers>
    {
        List<CampaignEndUsers> GetByUserWinCampaign(Campaign campaign, AccountEndUser accountEndUser);
        List<CampaignEndUsers> GetByUserCampaign(Campaign campaign, AccountEndUser accountEndUser);
    }
}
