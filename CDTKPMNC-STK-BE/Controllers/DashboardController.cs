using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using System;

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
        public DashboardController(PartnerService partnerService, EndUserService endUserService, StoreService storeService, CampaignService campaignService, ProductCategoryService productCategoryService, ProductItemService productItemService) 
        {
            _partnerService = partnerService;
            _endUserService = endUserService;
            _storeService = storeService;
            _campaignService = campaignService;
            _productCategoryService = productCategoryService;
            _ProductItemService = productItemService;
        }

        #region For admin
        // GET: <DashboardController>/Admin/PartnerCount
        [HttpGet("Admin/PartnerCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountPartner()
        {
            var nPartnerAll = _partnerService.CountAll();
            var nPartnerVerified = _partnerService.CountVerified();
            return Ok(new ResponseMessage(true, "OK", new { All = nPartnerAll, Verified = nPartnerVerified }));
        }

        // GET: <DashboardController>/Admin/StoreCount
        [HttpGet("Admin/StoreCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountStore()
        {
            var nAll = _storeService.CountAll();
            var nNeedApproved = _storeService.CountNeedApproved();
            var nApproved = _storeService.CountApproved();
            var nRejected = _storeService.CountRejected();
            return Ok(new ResponseMessage(true, "OK", new { All = nAll, NeedApproved = nNeedApproved, Approved  = nApproved, Rejected = nRejected }));

        }

        // GET: <DashboardController>/Admin/EndUserCount
        [HttpGet("Admin/EndUserCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountEndUser()
        {
            var nEndUserAll = _endUserService.CountAll();
            var nEndUserVerified = _endUserService.CountVerified();
            return Ok(new ResponseMessage(true, "OK", new { All = nEndUserAll, Verified = nEndUserVerified }));
        }

        // GET: <DashboardController>/Admin/CampaignCount
        [HttpGet("Admin/CampaignCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountAllCampaign()
        {
            var nCampaign = _campaignService.Count();
            return Ok(new ResponseMessage(true, "OK", new { All = nCampaign }));
        }

        // GET: <DashboardController>/Admin/CampaignCountByStatus
        [HttpGet("Admin/CampaignCountByStatus")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountByCampaignyStatus()
        {
            var campaignCount = _campaignService.CountByStatus();
            return Ok(new ResponseMessage(true, "OK", new { campaignCount }));
        }

        // GET: <DashboardController>/Admin/CampaignCountByStatus
        [HttpGet("Admin/CampaignCountByGame")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountByCampaignyGame()
        {
            var campaignCount = _campaignService.CountAllByGame();
            return Ok(new ResponseMessage(true, "OK", new { campaignCount }));
        }

        // GET: <DashboardController>/Admin/CategoryCount
        [HttpGet("Admin/ProductCategoryCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountProductCategoryAll()
        {
            var nCategory = _productCategoryService.CountAll();
            return Ok(new ResponseMessage(true, "OK", new { nCategory }));
        }

        // GET: <DashboardController>/Admin/ItemCount
        [HttpGet("Admin/ProductItemCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult ProductItemCountAll()
        {
            var nItem = _ProductItemService.CountAll();
            return Ok(new ResponseMessage(true, "OK", new { nItem }));
        }

        // GET: <DashboardController>/Admin/ItemCountByCategory
        [HttpGet("Admin/ItemCountByCategory")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountProductItemByCategory()
        {
            var nItemByCategory = _ProductItemService.CountByCaregory();
            return Ok(new ResponseMessage(true, "OK", new { nItemByCategory }));
        }

        #endregion

        #region For Partner

        // GET: <DashboardController>/Partner/CampaignCount
        [HttpGet("Partner/CampaignCount")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CountCampaign()
        {
            var nCampaign = _campaignService.Count(UserId);
            return Ok(new ResponseMessage(true, "OK", new { nCampaign }));
        }

        // GET: <DashboardController>/Partner/CampaignCountByStatus
        [HttpGet("Partner/CampaignCountByStatus")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CountByCampaignyStatusStore()
        {
            var campaignCount = _campaignService.CountByStatus(UserId);
            return Ok(new ResponseMessage(true, "OK", new { campaignCount }));
        }

        // GET: <DashboardController>/Partner/TotalNumberOfPlay
        [HttpGet("Partner/TotalNumberOfPlay")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CountNumberOfPlayStore()
        {
            var TotalNumberOfPlay = _campaignService.CountNumberOfPlay(UserId);
            return Ok(new ResponseMessage(true, "OK", new { TotalNumberOfPlay }));
        }

        // GET: <DashboardController>/Partner/TotalNumberOfPlayer
        [HttpGet("Partner/TotalNumberOfPlayer")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CountNumberOfPlayerStore()
        {
            var NumberOfPlayer = _campaignService.CountNumberOfPlayer(UserId);
            return Ok(new ResponseMessage(true, "OK", new { NumberOfPlayer }));
        }

        // GET: <DashboardController>/Partner/TotalNumberOfVoucher
        [HttpGet("Partner/TotalNumberOfVoucher")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CountNumberOfPVoucherStore()
        {
            var NumberOfPVoucher = _campaignService.CountNumberOfPVoucher(UserId);
            return Ok(new ResponseMessage(true, "OK", new { NumberOfPVoucher }));
        }

        // GET: <DashboardController>/Partner/ItemCount
        [HttpGet("Partner/ProductItemCount")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult ProductItemCountStore()
        {
            var nItem = _ProductItemService.CountAll(UserId);
            return Ok(new ResponseMessage(true, "OK", new { nItem }));
        }

        // GET: <DashboardController>/Partner/ItemCountByCategory
        [HttpGet("Partner/ItemCountByCategory")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CountProductItemByCategoryStore()
        {
            var nItemByCategory = _ProductItemService.CountByCaregory(UserId);
            return Ok(new ResponseMessage(true, "OK", new { nItemByCategory }));
        }

        #endregion

    }
}
