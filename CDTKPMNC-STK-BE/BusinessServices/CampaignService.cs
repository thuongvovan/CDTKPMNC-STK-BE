using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using FluentValidation;
using CDTKPMNC_STK_BE.BusinessServices.ModelConverter;
using System.Linq;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.DataAccess.Repositories.CampaignEndUsersRepository;
using System.Xml.Linq;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class CampaignService : CommonService
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly ICampaignVoucherSeriesRepository _campaignVoucherSeriesRepo;
        private readonly GameService _gameService;
        private readonly StoreService _storeService;
        private readonly VoucherService _voucherService;
        private readonly ICampaignEndUsersRepository _campaignEndUsersRepo;

        public CampaignService(IUnitOfWork unitOfWork, VoucherService voucherService, GameService gameService, StoreService storeService) : base(unitOfWork)
        {
            _campaignRepo = _unitOfWork.CampaignRepo;
            _campaignVoucherSeriesRepo = _unitOfWork.CampaignVoucherSeriesRepo;
            _campaignEndUsersRepo = _unitOfWork.CampaignEndUsersRepo;
            _gameService = gameService;
            _storeService = storeService;
            _voucherService = voucherService;
        }

        //WAITING,  Enable + trước thời gian
        //RUNNING,  Enable + trong thời gian
        //PENDING,  Disable + trước và trong thời gian
        //FINISHED, trong thời gian + hết voucher
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
            return CampaignConverter.ToCampaignReturn(campain);
        }

        public List<CampaignReturn> GetListCampaign()
        {
            var campaignList = _campaignRepo
                        .GetAll()
                        .Select(c => CampaignConverter.ToCampaignReturn(c)!)
                        .ToList();
            return campaignList;
        }

        public List<CampaignReturn> GetListCampaign(Guid storeId)
        {
            var campaignList = _campaignRepo
                        .GetAll()
                        .Where(c => c.StoreId == storeId)
                        .Select(c => CampaignConverter.ToCampaignReturn(c)!)
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
            var endDate = campaignCreateRecord.CampaignInfo!.EndDate!.ToDateTime();
            var name = campaignCreateRecord.CampaignInfo!.Name!;
            var isEnable = campaignCreateRecord.CampaignInfo!.IsEnable!.Value;
            var campaigns = store.Campaigns.Where(c => (c.IsEnable && isEnable && ((c.StartDate.ToDateTime() <= startDate && c.EndDate.ToDateTime() >= startDate) ||
                                                                                    (c.StartDate.ToDateTime() <= endDate && c.EndDate.ToDateTime() >= endDate))) ||
                                                        (c.Name.ToLower() == name.ToLower()));

            if (!campaigns!.Any())
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
                GameId = campaignCreateRecord!.CampaignInfo!.GameId!.Value,
                WinRate = campaignCreateRecord!.CampaignInfo!.WinRate!.Value,
                GameRule = campaignCreateRecord!.CampaignInfo!.GameRule!.Value,
                NumberOfLimit = campaignCreateRecord!.CampaignInfo!.NumberOfLimit,
                IsEnable = campaignCreateRecord!.CampaignInfo!.IsEnable!.Value
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
            return CampaignConverter.ToCampaignReturn(campaign)!;
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

        public bool VerifyDisableCampaign(Guid storeId, AccountType accountType, Campaign campaign)
        {
            if (accountType == AccountType.Admin)
            {
                return true;
            }
            else if (accountType == AccountType.Partner && campaign.StoreId == storeId)
            {
                return true;
            }
            return false;
        }

        public CampaignReturn DisableCampaign(Campaign campaign)
        {
            campaign.IsEnable = false;
            _campaignRepo.Update(campaign);
            return CampaignConverter.ToCampaignReturn(campaign)!;
        }

        public bool VerifyEnableCampaign(Guid storeId, AccountType accountType, Campaign campaign)
        {
            if (accountType == AccountType.Admin || (accountType == AccountType.Partner && campaign.StoreId == storeId))
            {
                var startDate = campaign.StartDate!.ToDateTime();
                var endDate = campaign.StartDate!.ToDateTime();
                var name = campaign!.Name!;

                var store = _storeService.GetById(campaign.StoreId);
                var campaigns = store!.Campaigns.Where(c => (c.IsEnable && ((c.StartDate.ToDateTime() <= startDate && c.EndDate.ToDateTime() >= startDate) ||
                                                                            (c.StartDate.ToDateTime() <= endDate && c.EndDate.ToDateTime() >= endDate))));
                if (!campaigns!.Any()) return true;
                if (campaigns!.Count() == 1)
                {
                    var otherCampaign = campaigns.First();
                    if (otherCampaign.Id == campaign.Id) return true;
                }
                return false;
            }
            return false;
        }

        public CampaignReturn EnableCampaign(Campaign campaign)
        {
            campaign.IsEnable = true;
            _campaignRepo.Update(campaign);
            return CampaignConverter.ToCampaignReturn(campaign)!;

        }

        /// <summary>
        /// Kiểm tra tính logic của CampaignInfoRecord với dữ liệu hiện tại
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="campaignInfoRecord"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public bool VerifyCampaignUpdateInfo(Campaign campaign, CampaignInfoRecord campaignInfoRecord, Guid storeId)
        {
            if (campaign.StoreId != storeId) return false;
            var store = _storeService.GetById(storeId);
            if (store == null) return false;
            if (store.Campaigns == null || store.Campaigns.Count == 0) return false;

            var startDate = campaignInfoRecord.StartDate!.ToDateTime();
            var endDate = campaignInfoRecord.StartDate!.ToDateTime();
            var name = campaignInfoRecord!.Name!;
            var isEnable = campaignInfoRecord!.IsEnable!.Value;

            var campaigns = store.Campaigns.Where(c => (c.IsEnable && isEnable && ((c.StartDate.ToDateTime() <= startDate && c.EndDate.ToDateTime() >= startDate) ||
                                                                                    (c.StartDate.ToDateTime() <= endDate && c.EndDate.ToDateTime() >= endDate))) ||
                                                        (c.Name.ToLower() == name.ToLower()));

            if (!campaigns!.Any()) return true;
            if (campaigns!.Count() == 1)
            {
                var otherCampaign = campaigns.First();
                if (otherCampaign.Id == campaign.Id) return true;
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
            campaign.WinRate = campaignInfoRecord!.WinRate!.Value;
            campaign.IsEnable = campaignInfoRecord!.IsEnable!.Value;
            campaign.GameRule = campaignInfoRecord!.GameRule!.Value;
            campaign.NumberOfLimit = null;
            if (campaign.GameRule == GameRule.Limit)
            {
                campaign.NumberOfLimit = campaignInfoRecord.NumberOfLimit;
            }
            _campaignRepo.Update(campaign);
            return CampaignConverter.ToCampaignReturn(campaign)!;
        }

        public CampaignVoucherSeriesReturn[] GetCampaignVoucherSeriesList(Campaign campaign)
        {
            return campaign.CampaignVoucherSeriesList.Select(cvs => CampaignConverter.ToCampaignVoucherSeriesReturn(cvs)).ToArray();
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

        public CampaignVoucherSeries? VerifyUpdateCampaignVoucherSeries(Campaign campaign, CampaignVoucherSeriesRecord campaignInfoRecord, Guid storeId)
        {
            if (campaign.StoreId != storeId) return null;
            var campaignVoucherSeries = campaign.CampaignVoucherSeriesList
                                                .FirstOrDefault(cvs => cvs.VoucherSeriesId == campaignInfoRecord.VoucherSeriesId);
            if (campaignVoucherSeries == null) return null;
            return campaignVoucherSeries;
        }

        public void UpdateCampaignVoucherSeries(Campaign campaign, CampaignVoucherSeries campaignVoucherSeries, CampaignVoucherSeriesRecord campaignVoucherSeriesRecord)
        {

            campaignVoucherSeries.CampaignId = campaign.Id;
            campaignVoucherSeries.VoucherSeriesId = campaignVoucherSeriesRecord.VoucherSeriesId!.Value;
            campaignVoucherSeries.Quantity = campaignVoucherSeriesRecord.Quantity!.Value;
            campaignVoucherSeries.ExpiresOn = campaignVoucherSeriesRecord.ExpiresOn!.ToDateOnly();
            _campaignRepo.Update(campaign);
        }

        public CampaignVoucherSeries? VerifyDeleteCampaignVoucherSeries(Campaign campaign, Guid voucherSeriesId, Guid storeId)
        {
            if (campaign.StoreId != storeId) return null;
            var campaignVoucherSeries = campaign.CampaignVoucherSeriesList
                                                .FirstOrDefault(cvs => cvs.CampaignId == campaign.Id &&
                                                                cvs.VoucherSeriesId == voucherSeriesId);
            if (campaignVoucherSeries == null) return null;
            if (campaignVoucherSeries.Vouchers.Any()) return null;
            return campaignVoucherSeries;
        }

        public void RemoveCampaignVoucherSeries(Campaign campaign, CampaignVoucherSeries cvs)
        {
            campaign.CampaignVoucherSeriesList.Remove(cvs);
            _campaignRepo.Update(campaign);
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
            campaign.CampaignVoucherSeriesList.Clear();
            _campaignRepo.Delete(campaign);
        }


        #region Check Is end user can join

        public CampaignEndUsers MarkEndUserJoined(Campaign campaign, Guid userId, bool isWinner)
        {
            var campaignEndUser = new CampaignEndUsers
            {
                Id = Guid.NewGuid(),
                Campaign = campaign,
                EndUserId = userId,
                GameId = campaign.GameId,
                IsWinner = isWinner
            };
            _campaignEndUsersRepo.Add(campaignEndUser);
            return campaignEndUser;
        }

        public bool CheckUserCanJoin(Campaign campaign, AccountEndUser endUser)
        {
            GameRule gameRule = campaign.GameRule;

            if (endUser == null) return false;
            if (gameRule == GameRule.Unlimited) return true;
            else if (gameRule == GameRule.UntilWin)
            {
                var campaignEndUsers = _campaignEndUsersRepo.GetByUserWinCampaign(campaign, endUser);
                if (campaignEndUsers != null && campaignEndUsers.Any())
                {
                    return false;
                }
                return true;
            }
            else if (gameRule == GameRule.Limit)
            {
                var campaignEndUsers = _campaignEndUsersRepo.GetByUserCampaign(campaign, endUser);
                if (campaignEndUsers != null && campaignEndUsers.Count >= campaign.NumberOfLimit!.Value)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        #endregion


        #region For dashboard

        public int Count()
        {
            return _campaignRepo.GetAll().Count();
        }

        public IEnumerable<(CampaignStatus, int)> CountByStatus()
        {
            var campaignList = GetListCampaign();
            var countGroup = campaignList.GroupBy( c => c.Status)
                                         .Select(g => (g.Key, g.Count()));
            return countGroup;
        }

        public IEnumerable<(Guid , string, int)> CountAllByGame()
        {
            var campaignList = GetListCampaign();
            var countGroup = campaignList.GroupBy(c => new { c.GameName, c.GameId })
                                         .Select(g => (g.Key.GameId, g.Key.GameName, g.Count()));
            return countGroup;
        }

        public int Count(Guid storeId)
        {
            return _campaignRepo.GetAll().Where(c => c.StoreId == storeId).Count();
        }

        public IEnumerable<(CampaignStatus, int)> CountByStatus(Guid storeId)
        {
            var campaignList = GetListCampaign(storeId);
            var countGroup = campaignList.GroupBy(c => c.Status)
                                         .Select(g => (g.Key, g.Count()));
            return countGroup;
        }

        public int CountNumberOfPlay(Guid storeId)
        {
            return _campaignEndUsersRepo.GetAll().Where(ce => ce.Campaign.StoreId == storeId) .Count();
        }

        public int CountNumberOfPlayer(Guid storeId)
        {
            return _campaignEndUsersRepo.GetAll()
                                        .Where(ce => ce.Campaign.StoreId == storeId)
                                        .Select(ce => ce.EndUserId)
                                        .Distinct()
                                        .Count();
        }

        public int CountNumberOfPVoucher(Guid storeId)
        {
            return _voucherService.GetVoucherPartner(storeId).Count;
        }

        #endregion

    }
}
