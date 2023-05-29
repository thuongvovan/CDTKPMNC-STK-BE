﻿using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess.Repositories.CampaignEndUsersRepository;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    /// <summary>
    /// Sử dụng chung để quản lý cho Voucher và CampaignVoucherSeriesList
    /// </summary>
    public class VoucherService : CommonService
    {
        private readonly IVoucherRepository _voucherRepo;
        private readonly IStoreRepository _storeRepository;
        private readonly IVoucherSeriesRepository _voucherSeriesRepo;
        private readonly IAccountEndUserRepository _accountEndUserRepo;
        private readonly ICampaignRepository _campaignRepo;


        public VoucherService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _voucherRepo = _unitOfWork.VoucherRepo;
            _voucherSeriesRepo = _unitOfWork.VoucherSeriesRepo;
            _storeRepository = _unitOfWork.StoreRepo;
            _accountEndUserRepo = _unitOfWork.AccountEndUserRepo;
            _campaignRepo = _unitOfWork.CampaignRepo;
        }

        public VoucherSeriesReturn? ToVoucherSeriesReturn(VoucherSeries? voucherSeries)
        {
            if (voucherSeries == null) return null;
            var voucherSeriesReturn = new VoucherSeriesReturn
            {
                Id = voucherSeries.Id,
                Name = voucherSeries.Name,
                Description = voucherSeries.Description,
                StoreId = voucherSeries.StoreId
            };
            return voucherSeriesReturn;
        }

        public VoucherSeries? GetVoucherSeries(Guid voucherSeriesId)
        {
            var voucherSeries = _voucherSeriesRepo.GetById(voucherSeriesId);
            return voucherSeries;
        }

        /// <summary>
        /// Lấy toàn bộ VoucherSeries
        /// </summary>
        /// <returns></returns>
        public List<VoucherSeriesReturn> GetListVoucherSeries()
        {
            var voucherSeries = _voucherSeriesRepo
                                .GetAll()
                                .Select(vs => ToVoucherSeriesReturn(vs)!)
                                .ToList();
            return voucherSeries;
        }

        /// <summary>
        /// Lấy toàn bộ VoucherSeries của một store
        /// </summary>
        /// <returns></returns>
        public List<VoucherSeriesReturn> GetListVoucherSeries(Guid storeId)
        {
            var voucherSeries = _voucherSeriesRepo
                                .GetAll()
                                .Where(vs => vs.StoreId == storeId)
                                .Select(vs => ToVoucherSeriesReturn(vs)!)
                                .ToList();
            return voucherSeries;
        }

        /// <summary>
        /// Kiểm tra giá trị nhập vào khi tạo mới có hợp lệ không
        /// </summary>
        /// <param name="voucherSeriesRecord"></param>
        /// <returns>ValidationSummary chứa status và mô tả lỗi</returns>
        public ValidationSummary ValidateVoucherSeriesRecord(VoucherSeriesRecord? voucherSeriesRecord)
        {
            if (voucherSeriesRecord == null)
            {
                return new ValidationSummary(false, "Voucher Series info is required.");
            }
            var validator = new VoucherSeriesRecordValidator();
            var result = validator.Validate(voucherSeriesRecord);
            return result.GetSummary();
        }

        /// <summary>
        /// Kiểm tra giá trị nhập vào với cơ sử dữ liêu có bị trùng không
        /// </summary>
        /// <param name="voucherSeriesRecord"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public bool VerifyVoucherSeriesRecord(Store store, VoucherSeriesRecord voucherSeriesRecord)
        {
            var voucherSeries = store.VoucherSeries.Where(vs => vs.Name.ToLower() == voucherSeriesRecord.Name!.ToLower());
            if (voucherSeries.Any()) return false;
            return true;
        }

        /// <summary>
        /// Kiểm tra giá trị nhập vào khi cập nhật có hợp lệ không
        /// </summary>
        /// <param name="voucherSeriesRecord"></param>
        /// <param name="storeId"></param>
        /// <returns>ValidationSummary chứa status và mô tả lỗi</returns>
        public VoucherSeries AddVoucherSeries(Store store, VoucherSeriesRecord voucherSeriesRecord)
        {
            var voucherSeries = new VoucherSeries
            {
                Name = voucherSeriesRecord.Name!.Trim(),
                Description = voucherSeriesRecord.Description!,
                CreatedAt = DateTime.Now
            };
            store.VoucherSeries.Add(voucherSeries);
            _storeRepository.Update(store);
            return voucherSeries;
        }

        /// <summary>
        /// Kiểm tra CampaignVoucherSeriesList có hợp lệ không và so sánh giá trị nhập vào với cơ sử dữ liêu có bị trùng không
        /// </summary>
        /// <param name="voucherSeriesRecord"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public bool VerifyUpdateVoucherSeries(Store store, VoucherSeries voucherSeries, VoucherSeriesRecord voucherSeriesRecord)
        {
            var otherVoucherSeries = store.VoucherSeries.SingleOrDefault(vs => vs.Name.ToLower() == voucherSeriesRecord.Name!.ToLower());
            if (otherVoucherSeries != null && otherVoucherSeries.Id != voucherSeries.Id) return false;
            return true;
        }

        /// <summary>
         /// Cập nhật thông tin cho CampaignVoucherSeriesList
        /// </summary>
        /// <param name="voucherSeries"></param>
        /// <param name="voucherSeriesRecord"></param>
        public void UpdateVoucherSeries(VoucherSeries voucherSeries, VoucherSeriesRecord voucherSeriesRecord)
        {
            voucherSeries.Name = voucherSeriesRecord.Name!;
            voucherSeries.Description = voucherSeriesRecord.Description!;
            _voucherSeriesRepo.Update(voucherSeries);
        }

        /// <summary>
        /// Kiểm tra xóa CampaignVoucherSeriesList hợp lệ không, nếu đã gán cho Campaign không được xóa
        /// </summary>
        /// <param name="voucherSeries"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public bool VerifyDeleteVoucherSeries(VoucherSeries voucherSeries)
        { 
            if (voucherSeries.CampaignVoucherSeriesList.Any()) return false;
            return true;
        }

        /// <summary>
        /// Thực hiện xóa CampaignVoucherSeriesList
        /// </summary>
        /// <param name="voucherSeries"></param>
        public void DeleteVoucherSeries(VoucherSeries voucherSeries)
        {
            _voucherSeriesRepo.Delete(voucherSeries);
        }

        public Voucher RandomVoucher(CampaignEndUsers campainEndUser)
        {
            var randomCampaignVoucherSeries = RandomCampaignVoucherSeries(campainEndUser.Campaign);
            var voucher = new Voucher
            {
                CampaignVoucherSeries = randomCampaignVoucherSeries,
                EndUserId = campainEndUser.EndUserId,
                CampaignEndUsers = campainEndUser,
                CreatedAt = DateTime.Now
            };
            randomCampaignVoucherSeries.Vouchers.Add(voucher);
            _voucherRepo.Add(voucher);
            return voucher;
        }

        public CampaignVoucherSeries RandomCampaignVoucherSeries(Campaign campaign)
        {
            var campaignVoucherList = new List<CampaignVoucherSeries>();
            foreach (var cvs in campaign.CampaignVoucherSeriesList)
            {
                var remainingAmount = cvs.Quantity - cvs.Vouchers.Count;
                for (int i = 0; i < remainingAmount; i++)
                {
                    campaignVoucherList.Add(cvs);
                }
            }
            var randomCampaignVoucherSeries = RandomHelper.GetRandomInArray(campaignVoucherList.ToArray());
            return randomCampaignVoucherSeries;
        }

        public VoucherReturn ToVoucherReturn(Voucher voucher)
        {
            var voucherReturn = new VoucherReturn
            {
                VoucherCode = voucher.VoucherCode,
                VoucherName = voucher.CampaignVoucherSeries.VoucherSeries.Name,
                Description = voucher.CampaignVoucherSeries.VoucherSeries.Description,
                StoreId = voucher.CampaignVoucherSeries.Campaign.StoreId,
                StoreName = voucher.CampaignVoucherSeries.Campaign.Store.Name,
                CampaignId = voucher.CampaignId,
                CampaignName = voucher.CampaignVoucherSeries.Campaign.Name,
                EndUserId = voucher.EndUserId,
                EndUserName = voucher.EndUser.Name!,
                ExpiresOn = voucher.CampaignVoucherSeries.ExpiresOn,
                IsUsed = voucher.IsUsed,
            };
            return voucherReturn;
        }

        public List<VoucherReturn> GetVoucherAll()
        {
            return _voucherRepo.GetAll().Select(v => ToVoucherReturn(v)).ToList();
        }

        public List<VoucherReturn> GetVoucherEndUser(Guid userId)
        {
            var endUser = _accountEndUserRepo.GetById(userId);
            return endUser!.Vouchers.Select(v => ToVoucherReturn(v)).ToList();
        }

        public List<VoucherReturn> GetVoucherPartner(Guid userId)
        {
            var store = _storeRepository.GetById(userId);
            if (store == null) return new List<VoucherReturn>(0);
            var campaigns = store!.Campaigns;
            var campaignVoucherSeriesList = campaigns.SelectMany(c => c.CampaignVoucherSeriesList)
                                                      .SelectMany(cvs => cvs.Vouchers)
                                                      .Select(v => ToVoucherReturn(v))
                                                      .ToList();
            return campaignVoucherSeriesList;
                
        }

        public List<VoucherReturn> GetVoucherStore(Store store)
        {
            var campaigns = store!.Campaigns;
            var campaignVoucherSeriesList = campaigns.SelectMany(c => c.CampaignVoucherSeriesList)
                                                      .SelectMany(cvs => cvs.Vouchers)
                                                      .Select(v => ToVoucherReturn(v))
                                                      .ToList();
            return campaignVoucherSeriesList;

        }

        public ValidationSummary ValidateVoucherShareRecord(VoucherShareRecord? voucherShareRecord)
        {
            if (voucherShareRecord == null)
            {
                return new ValidationSummary(false, "Voucher share info is required.");
            }
            var validator = new VoucherShareRecordValidator();
            var result = validator.Validate(voucherShareRecord);
            return result.GetSummary();
        }




        public void DeleteAllVouchers()
        {
            var vouchers = _voucherRepo.GetAll();
            foreach (var v in vouchers)
            {
                _voucherRepo.Delete(v);
            }
        }

    }
}
