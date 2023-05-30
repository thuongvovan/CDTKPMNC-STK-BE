using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.BusinessServices.ModelConverter
{
    static public class CampaignConverter
    {
        /// <summary>
        /// - WAITING,  Enable + trước thời gian
        /// - RUNNING,  Enable + trong thời gian
        /// - PENDING,  Disable + trước và trong thời gian
        /// - FINISHED, trong thời gian + hết voucher
        /// - EXPIRED   Sau thời gian
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        static public CampaignStatus GetCampaignStatus(Campaign campaign)
        {
            var startDate = campaign.StartDate.ToDateTime();
            var endDate = campaign.EndDate.ToDateTime();
            var totalQuantity = campaign.CampaignVoucherSeriesList.Sum(cvs => cvs.Quantity);
            var totalUsed = campaign.CampaignVoucherSeriesList.Sum(cvs => cvs.Vouchers.Count);
            if (totalUsed >= totalQuantity)
            {
                return CampaignStatus.FINISHED;
            }
            if (endDate < DateTime.Now)
            {
                return CampaignStatus.EXPIRED;
            }
            if (!campaign.IsEnable && endDate > DateTime.Now)
            {
                return CampaignStatus.PENDING;
            }
            if (campaign.IsEnable && startDate > DateTime.Now)
            {
                return CampaignStatus.WAITING;
            }
            if (campaign.IsEnable && startDate < DateTime.Now && endDate > DateTime.Now)
            {
                return CampaignStatus.RUNNING;
            }
            return CampaignStatus.UNKNOWN;
        }

        static public CampaignVoucherSeriesReturn ToCampaignVoucherSeriesReturn(CampaignVoucherSeries cvs)
        {
            return new CampaignVoucherSeriesReturn
            {
                Id = cvs.VoucherSeries.Id,
                Name = cvs.VoucherSeries.Name,
                Description = cvs.VoucherSeries.Description,
                CreatedAt = cvs.VoucherSeries.CreatedAt,
                Quantity = cvs.Quantity,
                QuantityUsed = cvs.Vouchers.Count,
                ExpiresOn = cvs.ExpiresOn
            };
        }

        static public CampaignReturn? ToCampaignReturn(Campaign? campaign)
        {
            if (campaign == null) return null;
            var campaignReturn = new CampaignReturn
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Description = campaign.Description,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                StoreId = campaign.StoreId,
                StoreName = campaign.Store.Name,
                GameId = campaign.GameId,
                GameName = campaign.Game.Name,
                WinRate = campaign.WinRate,
                CreatedAt = campaign.CreatedAt,
                IsEnable = campaign.IsEnable,
                Status = GetCampaignStatus(campaign),
                CampaignVoucherList = campaign.CampaignVoucherSeriesList
                                              .Select(cvs => ToCampaignVoucherSeriesReturn(cvs))
                                              .ToArray(),
                GameRule = campaign.GameRule,
                NumberOfLimit = campaign.NumberOfLimit
            };
            return campaignReturn;
        }
    }
}
