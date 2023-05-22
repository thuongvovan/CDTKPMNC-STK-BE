using System.Runtime.CompilerServices;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class CampaignVoucherSeriesRepository : CommonRepository<CampaignVoucherSeries>, ICampaignVoucherSeriesRepository
    {
        public CampaignVoucherSeriesRepository(AppDbContext context) : base(context)
        {
        }
    }
}
