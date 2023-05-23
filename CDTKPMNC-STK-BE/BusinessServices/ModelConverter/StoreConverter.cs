using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.BusinessServices.ModelConverter
{
    static public class StoreConverter
    {
        static public StoreReturn_E ToStoreReturn_E(Store store)
        {
            var storeCampaign = store.Campaigns
                                     .Select(c => CampaignConverter.ToCampaignReturn(c)!)
                                     .FirstOrDefault(c => c.Status == CampaignStatus.RUNNING);
            return new StoreReturn_E
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                Address = store.Address,
                OpenTime = store.OpenTime,
                CloseTime = store.CloseTime,
                BannerUrl = store.BannerUrl,
                Campaign = storeCampaign
            };
        }
    }
}
