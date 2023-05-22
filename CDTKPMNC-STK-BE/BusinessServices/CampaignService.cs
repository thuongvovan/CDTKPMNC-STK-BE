using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using FluentValidation;
using System.Linq;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class CampaignService : CommonService
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly ICampaignVoucherSeriesRepository _campaignVoucherSeriesRepo;
        private readonly GameService _gameService;
        private readonly StoreService _storeService;
        private readonly VoucherService _voucherService;
        public CampaignService(IUnitOfWork unitOfWork, VoucherService voucherService, GameService gameService, StoreService storeService) : base(unitOfWork)
        {
            _campaignRepo = _unitOfWork.CampaignRepo;
            _campaignVoucherSeriesRepo = _unitOfWork.CampaignVoucherSeriesRepo;
            _gameService = gameService;
            _storeService = storeService;
            _voucherService = voucherService;
        }

        //WAITING,  Enable + trước thời gian
        //RUNNING,  Enable + trong thời gian
        //PENDING,  Disable + trước và trong thời gian
        //FINISHED, // trong thời gian + hết voucher
        //EXPIRED   Sau thời gian

        public CampaignStatus GetCampaignStatus(Campaign campaign)
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

        public CampaignVoucherSeriesReturn ToCampaignVoucherSeriesReturn(CampaignVoucherSeries cvs)
        {
            return new CampaignVoucherSeriesReturn
            {
                Id = cvs.VoucherSeries.Id,
                Name = cvs.VoucherSeries.Name,
                Description = cvs.VoucherSeries.Description,
                CreatedAt = cvs.VoucherSeries.CreatedAt,
                Quantity = cvs.Quantity,
                QuantityUsed = cvs.Vouchers.Count,
            };
        }

        public CampaignReturn? ToCampaignReturn(Campaign? campaign)
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
                CreatedAt = campaign.CreatedAt,
                IsEnable = campaign.IsEnable,
                Status = GetCampaignStatus(campaign),
                CampaignVoucherList = campaign.CampaignVoucherSeriesList
                                              .Select(cvs => ToCampaignVoucherSeriesReturn(cvs))
                                              .ToArray()
            };
            return campaignReturn;
        }

        public Campaign? GetCampaign(Guid campaignId)
        {
            var campain = _campaignRepo.GetById(campaignId);
            return campain;
        }

        public CampaignReturn? GetCampaignReturn(Guid campaignId)
        {
            var campain = _campaignRepo.GetById(campaignId);
            return ToCampaignReturn(campain);
        }

        public List<CampaignReturn> GetListCampaign() 
        {
            var campaignList = _campaignRepo
                        .GetAll()
                        .Select(c => ToCampaignReturn(c)!)
                        .ToList();
            return campaignList;
        }

        public List<CampaignReturn> GetListCampaign(Guid storeId)
        {
            var campaignList = _campaignRepo
                        .GetAll()
                        .Where(c => c.StoreId == storeId)
                        .Select(c => ToCampaignReturn(c)!)
                        .ToList();
            return campaignList;
        }

        public ValidationSummary ValidateCampaignCreateRecord(CampaignCreateRecord? campaignCreateRecord)
        {
            if (campaignCreateRecord == null)
            {
                return new ValidationSummary(false, "Campaign is required.");
            }
            var validator = new CampaignCreateRecordValidator(_gameService, _voucherService);
            var result = validator.Validate(campaignCreateRecord);
            return result.GetSummary();
        }

        public bool VerifyCampaignCreateRecord(Guid storeId, CampaignCreateRecord campaignCreateRecord)
        {
            var store = _storeService.GetById(storeId);
            if (store == null) return false;
            if (store.Campaigns == null || store.Campaigns.Count == 0)
            {
                return true;
            }
            var startDate = campaignCreateRecord.CampaignInfo!.StartDate!.ToDateTime();
            var endDate = campaignCreateRecord.CampaignInfo!.StartDate!.ToDateTime();
            var campaigns = store.Campaigns.Where(c => (c.StartDate.ToDateTime() >= startDate && c.StartDate.ToDateTime() <= endDate) ||
                                                        (c.EndDate.ToDateTime() >= startDate && c.EndDate.ToDateTime() <= endDate));
            
            if (campaigns == null || (campaigns != null && campaigns!.Any()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Khởi tạo Campaign từ CampaignCreateRecord
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="campaignCreateRecord"></param>
        /// <returns></returns>
        public CampaignReturn CreateCampaign(Guid storeId, CampaignCreateRecord campaignCreateRecord)
        {
            var campaign = new Campaign
            {
                CreatedAt = DateTime.Now,
                StoreId = storeId,
                Name = campaignCreateRecord!.CampaignInfo!.Name!,
                Description = campaignCreateRecord!.CampaignInfo!.Description!,
                EndDate = campaignCreateRecord!.CampaignInfo!.EndDate!.ToDateOnly(),
                StartDate = campaignCreateRecord!.CampaignInfo!.StartDate!.ToDateOnly(),
                GameId = campaignCreateRecord!.CampaignInfo!.GameId!.Value
            };
            foreach (var voucherSeriesCampaignRecord in campaignCreateRecord!.CampaignVoucherSeriesList!)
            {
                var voucherSeriesCampaign = new CampaignVoucherSeries
                {
                    VoucherSeriesId = voucherSeriesCampaignRecord.VoucherSeriesId!.Value,
                    Quantity = voucherSeriesCampaignRecord.Quantity!.Value,
                    ExpiresOn = voucherSeriesCampaignRecord.ExpiresOn!.ToDateOnly(),
                };
                campaign.CampaignVoucherSeriesList.Add(voucherSeriesCampaign);
            }
            _campaignRepo.Add(campaign);
            return ToCampaignReturn(campaign)!;
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của CampaignInfoRecord
        /// </summary>
        /// <param name="campaignInfoRecord"></param>
        /// <returns></returns>
        public ValidationSummary ValidateCampaignInfoRecord(CampaignInfoRecord? campaignInfoRecord)
        {
            if (campaignInfoRecord == null)
            {
                return new ValidationSummary(false, "Campaign is required.");
            }
            var validator = new CampaignInfoRecordValidator(_gameService);
            var result = validator.Validate(campaignInfoRecord);
            return result.GetSummary();
        }

        /// <summary>
        /// Kiểm tra tính logic của CampaignInfoRecord với dữ liệu hiện tại
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="campaignInfoRecord"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public bool VerifyCampaignInfoRecord(Campaign campaign, CampaignInfoRecord campaignInfoRecord, Guid storeId)
        {
            if (campaign.StoreId != storeId) return false;
            var store = _storeService.GetById(storeId);
            if (store == null) return false;
            if (store.Campaigns == null || store.Campaigns.Count == 0)
            {
                return false;
            }
            var startDate = campaignInfoRecord.StartDate!.ToDateTime();
            var endDate = campaignInfoRecord.StartDate!.ToDateTime();
            var campaigns = store.Campaigns.Where(c =>  (c.StartDate.ToDateTime() >= startDate && c.StartDate.ToDateTime() <= endDate) ||
                                                        (c.EndDate.ToDateTime() >= startDate && c.EndDate.ToDateTime() <= endDate));
            if (campaigns == null || (campaigns != null && campaigns!.Any()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Cập nhật thông tin của Campaign
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="campaignInfoRecord"></param>
        public CampaignReturn UpdateCampaignInfo(Campaign campaign, CampaignInfoRecord campaignInfoRecord)
        {
            campaign.Name = campaignInfoRecord!.Name!;
            campaign.Description = campaignInfoRecord!.Description!;
            campaign.EndDate = campaignInfoRecord!.EndDate!.ToDateOnly();
            campaign.StartDate = campaignInfoRecord!.StartDate!.ToDateOnly();
            campaign.GameId = campaignInfoRecord!.GameId!.Value;
            _campaignRepo.Update(campaign);
            return ToCampaignReturn(campaign)!;
        }

        public CampaignVoucherSeriesReturn[] GetCampaignVoucherSeriesList(Campaign campaign)
        {
            return campaign.CampaignVoucherSeriesList.Select(cvs => ToCampaignVoucherSeriesReturn(cvs)).ToArray();
        }

        public ValidationSummary ValidateCampaignVoucherSeriesRecord(CampaignVoucherSeriesRecord? voucherSeriesCampaignRecord)
        {
            if (voucherSeriesCampaignRecord == null)
            {
                return new ValidationSummary(false, "Campaign Voucher Series is required.");
            }
            var validator = new CampaignVoucherSeriesRecordValidator(_voucherService);
            var result = validator.Validate(voucherSeriesCampaignRecord);
            return result.GetSummary();
        }

        public bool VerifyUpdateCampaignVoucherSeries(Campaign campaign, CampaignVoucherSeriesRecord campaignInfoRecord, Guid storeId)
        {
            if (campaign.StoreId != storeId) return false;
            var campaignVoucherSeries = campaign.CampaignVoucherSeriesList
                                                .FirstOrDefault(cvs => cvs.CampaignId == campaign.Id &&
                                                                cvs.VoucherSeriesId == campaignInfoRecord.VoucherSeriesId);
            if (campaignVoucherSeries == null) return false;
            return true;
        }

        public void UpdateCampaignVoucherSeries(Campaign campaign, CampaignVoucherSeriesRecord campaignVoucherSeriesRecord)
        {
            _campaignVoucherSeriesRepo.Update(new CampaignVoucherSeries
            {
                CampaignId = campaign.Id,
                VoucherSeriesId = campaignVoucherSeriesRecord.VoucherSeriesId!.Value,
                Quantity = campaignVoucherSeriesRecord.Quantity!.Value,
                ExpiresOn = campaignVoucherSeriesRecord.ExpiresOn!.ToDateOnly(),
            });
        }

        public bool VerifyDeleteCampaignVoucherSeries(Campaign campaign, Guid voucherSeriesId, Guid storeId)
        {
            if (campaign.StoreId != storeId) return false;
            var campaignVoucherSeries = campaign.CampaignVoucherSeriesList
                                                .FirstOrDefault(cvs => cvs.CampaignId == campaign.Id &&
                                                                cvs.VoucherSeriesId == voucherSeriesId);
            if (campaignVoucherSeries == null) return false;
            if (campaignVoucherSeries.Vouchers.Any()) return false;
            return true;
        }

        public void RemoveCampaignVoucherSeries(Campaign campaign, Guid voucherSeriesId)
        {
            _campaignVoucherSeriesRepo.Delete( new CampaignVoucherSeries { CampaignId = campaign.Id, VoucherSeriesId = voucherSeriesId} );
        }

        public bool VerifyAddCampaignVoucherSeries(Campaign campaign, CampaignVoucherSeriesRecord campaignInfoRecord, Guid storeId)
        {
            if (campaign.StoreId != storeId) return false;
            var campaignVoucherSeries = campaign.CampaignVoucherSeriesList
                                                .FirstOrDefault(cvs => cvs.CampaignId == campaign.Id &&
                                                                cvs.VoucherSeriesId == campaignInfoRecord.VoucherSeriesId);
            if (campaignVoucherSeries == null) return true;
            return false;
        }

        public void AddCampaignVoucherSeries(Campaign campaign, CampaignVoucherSeriesRecord campaignVoucherSeriesRecord)
        {
            _campaignVoucherSeriesRepo.Add(new CampaignVoucherSeries
            {
                CampaignId = campaign.Id,
                VoucherSeriesId = campaignVoucherSeriesRecord.VoucherSeriesId!.Value,
                Quantity = campaignVoucherSeriesRecord.Quantity!.Value,
                ExpiresOn = campaignVoucherSeriesRecord.ExpiresOn!.ToDateOnly(),
            });
        }

        public bool VerifyDeleteCampaign(Campaign campaign)
        {
            foreach (var campaignVoucherSeries in campaign.CampaignVoucherSeriesList)
            {
                if (campaignVoucherSeries.Vouchers.Any())
                {
                    return false;
                }
            }
            return true;
        }

        public void RemoveCampaign(Campaign campaign)
        {
            _campaignRepo.Delete(campaign);
        }

    }
}
