using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Crypto;
using System;
using static CDTKPMNC_STK_BE.Utilities.CacheHelper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : CommonController
    {
        private readonly PartnerService _partnerService;
        private readonly EndUserService _endUserService;
        private readonly StoreService _storeService;
        private readonly CampaignService _campaignService;
        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductItemService _ProductItemService;
        private readonly IDistributedCache _cache;

        public DashboardController(PartnerService partnerService, EndUserService endUserService, StoreService storeService, CampaignService campaignService, ProductCategoryService productCategoryService, ProductItemService productItemService, IDistributedCache cache) 
        {
            _partnerService = partnerService;
            _endUserService = endUserService;
            _storeService = storeService;
            _campaignService = campaignService;
            _productCategoryService = productCategoryService;
            _ProductItemService = productItemService;
            _cache = cache;
        }

        #region For admin
        // GET: <DashboardController>/Admin/PartnerCount
        [HttpGet("Admin/PartnerCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountPartner()
        {
            var cacheId = $"{UserId}_Dashboard_CountPartner";
            var returnData = await _cache.GetRecordAsync<PartnerCount>(cacheId);
            if (returnData == null)
            {
                var nPartnerAll = _partnerService.CountAll();
                var nPartnerVerified = _partnerService.CountVerified();
                returnData = new PartnerCount(nPartnerAll, nPartnerVerified);
                await _cache.SetRecordAsync(cacheId, returnData, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", returnData));
        }


        // GET: <DashboardController>/Admin/StoreCount
        [HttpGet("Admin/StoreCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountStore()
        {
            var cacheId = $"{UserId}_Dashboard_StoreCount";
            var returnData = await _cache.GetRecordAsync<StoreCount>(cacheId);
            if (returnData == null)
            {
                var nAll = _storeService.CountAll();
                var nNeedApproved = _storeService.CountNeedApproved();
                var nApproved = _storeService.CountApproved();
                var nRejected = _storeService.CountRejected();
                returnData = new StoreCount(nAll, nNeedApproved, nApproved, nRejected);
                await _cache.SetRecordAsync(cacheId, returnData, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", returnData));
        }

        // GET: <DashboardController>/Admin/EndUserCount
        [HttpGet("Admin/EndUserCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountEndUser()
        {
            var cacheId = $"{UserId}_Dashboard_EndUserCount";
            var returnData = await _cache.GetRecordAsync<EndUserCount>(cacheId);
            if (returnData == null)
            {
                var nEndUserAll = _endUserService.CountAll();
                var nEndUserVerified = _endUserService.CountVerified();
                returnData = new EndUserCount(nEndUserAll, nEndUserVerified);
                await _cache.SetRecordAsync(cacheId, returnData, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", returnData));
        }

        // GET: <DashboardController>/Admin/CampaignCount
        [HttpGet("Admin/CampaignCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountAllCampaign()
        {
            var cacheId = $"{UserId}_Dashboard_CampaignCount";
            var nCampaign = await _cache.GetRecordAsync<int?>(cacheId);
            if (nCampaign == null)
            {
                nCampaign = _campaignService.Count();
                await _cache.SetRecordAsync(cacheId, nCampaign, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { All = nCampaign }));
        }

        // GET: <DashboardController>/Admin/CampaignCountByStatus
        [HttpGet("Admin/CampaignCountByStatus")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountByCampaignyStatus()
        {
            var cacheId = $"{UserId}_Dashboard_CampaignCountByStatus";
            var campaignCount = await _cache.GetRecordAsync<IEnumerable<(CampaignStatus, int)>>(cacheId);
            if (campaignCount == null)
            {
                campaignCount = _campaignService.CountByStatus();
                await _cache.SetRecordAsync(cacheId, campaignCount, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { campaignCount }));
        }

        // GET: <DashboardController>/Admin/CampaignCountByStatus
        [HttpGet("Admin/CampaignCountByGame")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountByCampaignyGame()
        {
            var cacheId = $"{UserId}_Dashboard_CampaignCountByGame";
            var campaignCount = await _cache.GetRecordAsync<IEnumerable<(Guid, string ,int)>>(cacheId);
            if (campaignCount == null)
            {
                campaignCount = _campaignService.CountAllByGame();
                await _cache.SetRecordAsync(cacheId, campaignCount, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { campaignCount }));   
        }

        // GET: <DashboardController>/Admin/CategoryCount
        [HttpGet("Admin/ProductCategoryCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountProductCategoryAll()
        {
            var cacheId = $"{UserId}_Dashboard_ProductCategoryCount";
            var nCategory = await _cache.GetRecordAsync<int?>(cacheId);
            if (nCategory == null)
            {
                nCategory = _productCategoryService.CountAll();
                await _cache.SetRecordAsync(cacheId, nCategory, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { nCategory }));
        }

        // GET: <DashboardController>/Admin/ItemCount
        [HttpGet("Admin/ProductItemCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task< IActionResult> ProductItemCountAll()
        {
            var cacheId = $"{UserId}_Dashboard_ProductItemCount";
            var nItem = await _cache.GetRecordAsync<int?>(cacheId);
            if (nItem == null)
            {
                nItem = _ProductItemService.CountAll();
                await _cache.SetRecordAsync(cacheId, nItem, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { nItem }));
        }

        // GET: <DashboardController>/Admin/ItemCountByCategory
        [HttpGet("Admin/ItemCountByCategory")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CountProductItemByCategory()
        {
            var cacheId = $"{UserId}_Dashboard_ItemCountByCategory";
            var nItemByCategory = await _cache.GetRecordAsync<IEnumerable<(Guid, string,int)>>(cacheId);
            if (nItemByCategory == null)
            {
                nItemByCategory = _ProductItemService.CountByCaregory();
                await _cache.SetRecordAsync(cacheId, nItemByCategory, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { nItemByCategory }));
        }

        #endregion

        #region For Partner

        // GET: <DashboardController>/Partner/CampaignCount
        [HttpGet("Partner/CampaignCount")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public async Task<IActionResult> CountCampaign()
        {
            var cacheId = $"{UserId}_Dashboard_CampaignCount";
            var nCampaign = await _cache.GetRecordAsync<int?>(cacheId);
            if (nCampaign == null)
            {
                nCampaign = _campaignService.Count(UserId);
                await _cache.SetRecordAsync(cacheId, nCampaign, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { nCampaign }));
        }

        // GET: <DashboardController>/Partner/CampaignCountByStatus
        [HttpGet("Partner/CampaignCountByStatus")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public async Task<IActionResult> CountByCampaignyStatusStore()
        {
            var cacheId = $"{UserId}_Dashboard_CampaignCountByStatus";
            var campaignCount = await _cache.GetRecordAsync<IEnumerable<(CampaignStatus, int)>>(cacheId);
            if (campaignCount == null)
            {
                campaignCount = _campaignService.CountByStatus(UserId);
                await _cache.SetRecordAsync(cacheId, campaignCount, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { campaignCount }));
        }

        // GET: <DashboardController>/Partner/TotalNumberOfPlay
        [HttpGet("Partner/TotalNumberOfPlay")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public async Task<IActionResult> CountNumberOfPlayStore()
        {
            var cacheId = $"{UserId}_Dashboard_TotalNumberOfPlay";
            var TotalNumberOfPlay = await _cache.GetRecordAsync<int?>(cacheId);
            if (TotalNumberOfPlay == null)
            {
                TotalNumberOfPlay = _campaignService.CountNumberOfPlay(UserId);
                await _cache.SetRecordAsync(cacheId, TotalNumberOfPlay, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { TotalNumberOfPlay }));
        }

        // GET: <DashboardController>/Partner/TotalNumberOfPlayer
        [HttpGet("Partner/TotalNumberOfPlayer")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public async Task<IActionResult> CountNumberOfPlayerStore()
        {
            var cacheId = $"{UserId}_Dashboard_TotalNumberOfPlayer";
            var NumberOfPlayer = await _cache.GetRecordAsync<int?>(cacheId);
            if (NumberOfPlayer == null)
            {
                NumberOfPlayer = _campaignService.CountNumberOfPlayer(UserId);
                await _cache.SetRecordAsync(cacheId, NumberOfPlayer, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { NumberOfPlayer }));
        }

        // GET: <DashboardController>/Partner/TotalNumberOfVoucher
        [HttpGet("Partner/TotalNumberOfVoucher")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public async Task<IActionResult> CountNumberOfPVoucherStore()
        {
            var cacheId = $"{UserId}_Dashboard_TotalNumberOfVoucher";
            var NumberOfPVoucher = await _cache.GetRecordAsync<int?>(cacheId);
            if (NumberOfPVoucher == null)
            {
                NumberOfPVoucher = _campaignService.CountNumberOfPVoucher(UserId);
                await _cache.SetRecordAsync(cacheId, NumberOfPVoucher, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { NumberOfPVoucher }));
        }

        // GET: <DashboardController>/Partner/ItemCount
        [HttpGet("Partner/ProductItemCount")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public async Task<IActionResult> ProductItemCountStore()
        {
            var cacheId = $"{UserId}_Dashboard_ProductItemCount";
            var nItem = await _cache.GetRecordAsync<int?>(cacheId);
            if (nItem == null)
            {
                nItem = _ProductItemService.CountAll(UserId);
                await _cache.SetRecordAsync(cacheId, nItem, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { nItem }));
        }

        // GET: <DashboardController>/Partner/ItemCountByCategory
        [HttpGet("Partner/ItemCountByCategory")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public async Task<IActionResult> CountProductItemByCategoryStore()
        {
            var cacheId = $"{UserId}_Dashboard_ItemCountByCategory";
            var nItemByCategory = await _cache.GetRecordAsync<IEnumerable<(Guid, string, int)>>(cacheId);
            if (nItemByCategory == null)
            {
                nItemByCategory = _ProductItemService.CountByCaregory(UserId);
                await _cache.SetRecordAsync(cacheId, nItemByCategory, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage(true, "OK", new { nItemByCategory }));
        }

        #endregion

    }
}
