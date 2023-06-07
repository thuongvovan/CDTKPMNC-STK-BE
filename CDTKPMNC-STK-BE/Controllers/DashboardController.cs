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
    public class DashboardController : ControllerBase
    {
        private readonly PartnerService _partnerService;
        private readonly EndUserService _endUserService;
        private readonly StoreService _storeService;
        private readonly CampaignService _campaignService;
        public DashboardController(PartnerService partnerService, EndUserService endUserService, StoreService storeService, CampaignService campaignService) 
        {
            _partnerService = partnerService;
            _endUserService = endUserService;
            _storeService = storeService;
            _campaignService = campaignService;
        }

        #region For admin
        // GET: <DashboardController>/PartnerCount
        [HttpGet("PartnerCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountPartner()
        {
            var nPartnerAll = _partnerService.CountAll();
            var nPartnerVerified = _partnerService.CountVerified();
            return Ok(new ResponseMessage(true, "OK", new { All = nPartnerAll, Verified = nPartnerVerified }));
        }

        // GET: <DashboardController>/StoreCount
        [HttpGet("StoreCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountStore()
        {
            var nAll = _storeService.CountAll();
            var nNeedApproved = _storeService.CountNeedApproved();
            var nApproved = _storeService.CountApproved();
            var nRejected = _storeService.CountRejected();
            return Ok(new ResponseMessage(true, "OK", new { All = nAll, NeedApproved = nNeedApproved, Approved  = nApproved, Rejected = nRejected }));

        }

        // GET: <DashboardController>/EndUserCount
        [HttpGet("EndUserCount")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountEndUser()
        {
            var nEndUserAll = _endUserService.CountAll();
            var nEndUserVerified = _endUserService.CountVerified();
            return Ok(new ResponseMessage(true, "OK", new { All = nEndUserAll, Verified = nEndUserVerified }));
        }

        // GET: <DashboardController>/CampaignCountAll
        [HttpGet("CampaignCountAll")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountAllCampaign()
        {
            var nCampaign = _campaignService.CountAll();
            return Ok(new ResponseMessage(true, "OK", new { All = nCampaign }));
        }

        // GET: <DashboardController>/CampaignCountByStatus
        [HttpGet("CampaignCountByStatus")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountBCampaignyStatus()
        {
            var campaignCount = _campaignService.CountAllByStatus();
            return Ok(new ResponseMessage(true, "OK", campaignCount));
        }

        // GET: <DashboardController>/CampaignCountByStatus
        [HttpGet("CampaignCountByGame")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult CountByCampaignyGame()
        {
            var campaignCount = _campaignService.CountAllByGame();
            return Ok(new ResponseMessage(true, "OK", campaignCount));
        }
        #endregion

    }
}
