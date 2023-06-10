using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CampaignController : CommonController
    {
        private readonly CampaignService _campaignService;
        private readonly StoreService _storeService;
        private readonly EndUserService _endUserService;
        private readonly IDistributedCache _cache;
        public CampaignController(CampaignService campaignService, StoreService storeService, EndUserService endUserService, IDistributedCache cache)
        {
            _campaignService = campaignService;
            _storeService = storeService;
            _endUserService = endUserService;
            _cache = cache;
        }

        // GET: <CampaignController>/All
        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public async Task<IActionResult> GetListCampaign()
        {
            // Lay cache
            var cacheId = $"{UserId}_Campaign_All";
            var listCampaign = await _cache.GetRecordAsync<List<CampaignReturn>?>(cacheId);
            if (listCampaign != null)
            {
                if (UserType == AccountType.Admin)
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all campaign.", Data = new { ListCampaign = listCampaign } });
                }
                else if (UserType == AccountType.Partner)
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all your store campaign.", Data = new { ListCampaign = listCampaign } });
                }
            }
            // Khong co cache
            if (UserType == AccountType.Admin)
            {
                listCampaign = _campaignService.GetListCampaign();
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all campaign.", Data = new { ListCampaign = listCampaign } });
            }
            else if (UserType == AccountType.Partner)
            {
                listCampaign = _campaignService.GetListCampaign(UserId);
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all your store campaign.", Data = new { ListCampaign = listCampaign } });
            }
            await _cache.SetRecordAsync(cacheId, listCampaign, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2)).ConfigureAwait(false);
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }

        // GET: <CampaignController>/All/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpGet("All/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetListCampaign(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Store does not exist." });
            }
            var listCampaign = _campaignService.GetListCampaign(storeId);
            if (UserType == AccountType.Admin)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all campaign.", Data = new { ListCampaign = listCampaign } });
            }
            else if (UserType == AccountType.Partner && UserId == storeId)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all your store campaign.", Data = new { ListCampaign = listCampaign } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }

        // POST: <CampaignController>/Create
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CreateCampaign([FromBody] CampaignCreateRecord campaignCreateRecord)
        {
            var validateSummary = _campaignService.ValidateCampaignCreateRecord(campaignCreateRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            bool IsVerified = _campaignService.VerifyCampaignCreateRecord(UserId, campaignCreateRecord);
            if (IsVerified)
            {

                var campaign = _campaignService.CreateCampaign(UserId, campaignCreateRecord);
                return Ok(new ResponseMessage { Success = true, Message = "Create new campaign successfuly.", Data = new { Campaign = campaign } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }

        // GET: <CampaignController>/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpGet("{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetCampaign(Guid campaignId)
        {
            var campaign = _campaignService.GetCampaignReturn(campaignId);
            if (campaign != null)
            {
                if (UserType == AccountType.Admin || (UserType == AccountType.Partner && campaign.StoreId == UserId))
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Get campaign successfuly.", Data = new { Campaign = campaign } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }

        // DELETE: <CampaignController>/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpDelete("{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult DeleteCampaign(Guid campaignId)
        {
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                bool IsVerified = _campaignService.VerifyDeleteCampaign(campaign);
                if (IsVerified)
                {
                    _campaignService.RemoveCampaign(campaign);
                    return Ok(new ResponseMessage { Success = true, Message = "Delete Campaign successfuly." });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid delete request." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }


        // PUT: <CampaignController>/Disable/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPut("Disable/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult DisableCampaign(Guid campaignId)
        {
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                bool IsVerified = _campaignService.VerifyDisableCampaign(UserId, UserType, campaign);
                if (IsVerified)
                {
                    var campaignReturn = _campaignService.DisableCampaign(campaign);
                    return Ok(new ResponseMessage { Success = true, Message = "Disable Campaign successfuly.", Data = new { Campaign = campaignReturn } });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }


        // PUT: <CampaignController>/Enable/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPut("Enable/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult EnableCampaign(Guid campaignId)
        {
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                bool IsVerified = _campaignService.VerifyEnableCampaign(UserId, UserType, campaign);
                if (IsVerified)
                {
                    var campaignReturn = _campaignService.EnableCampaign(campaign);
                    return Ok(new ResponseMessage { Success = true, Message = "Enable Campaign successfuly.", Data = new { Campaign = campaignReturn } });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }

        // PUT: <CampaignController>/Info/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPut("Info/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult UpdateCampaignInfo(Guid campaignId, [FromBody] CampaignInfoRecord campaignInfoRecord)
        {
            var validateSummary = _campaignService.ValidateCampaignInfoRecord(campaignInfoRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                bool IsVerified = _campaignService.VerifyCampaignUpdateInfo(campaign, campaignInfoRecord, UserId);
                if (IsVerified)
                {
                    var campainReturn = _campaignService.UpdateCampaignInfo(campaign, campaignInfoRecord);
                    return Ok(new ResponseMessage { Success = true, Message = "Update Campaign successfuly.", Data = new { Campaign = campainReturn } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }

        // GET: <CampaignController>/Voucher/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpGet("Voucher/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult GetCampaignVoucherSeries(Guid campaignId)
        {
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null && campaign.StoreId == UserId)
            {
                var campaignVoucherSeriesList = _campaignService.GetCampaignVoucherSeriesList(campaign);
                return Ok(new ResponseMessage { Success = true, Message = "Get Campaign Voucher Series successfuly.", Data = new { CampaignVoucherSeriesList = campaignVoucherSeriesList } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }

        // POST: <CampaignController>/Voucher/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPost("Voucher/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult AddCampaignVoucherSeries(Guid campaignId, [FromBody] CampaignVoucherSeriesRecord campaignVoucherSeriesRecord)
        {
            var validateSummary = _campaignService.ValidateCampaignVoucherSeriesRecord(campaignVoucherSeriesRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                bool IsVerified = _campaignService.VerifyAddCampaignVoucherSeries(campaign, campaignVoucherSeriesRecord, UserId);
                if (IsVerified)
                {
                    _campaignService.AddCampaignVoucherSeries(campaign, campaignVoucherSeriesRecord);
                    var campaignVoucherSeriesList = _campaignService.GetCampaignVoucherSeriesList(campaign);
                    return Ok(new ResponseMessage { Success = true, Message = "Add Campaign Voucher Series successfuly.", Data = new { CampaignVoucherSeriesList = campaignVoucherSeriesList } });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Campaign Voucher Series is really existed." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }

        // PUT: <CampaignController>/Voucher/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPut("Voucher/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult UpdateCampaignVoucherSeries(Guid campaignId, [FromBody] CampaignVoucherSeriesRecord campaignVoucherSeriesRecord)
        {
            var validateSummary = _campaignService.ValidateCampaignVoucherSeriesRecord(campaignVoucherSeriesRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                var campaignVoucherSeries = _campaignService.VerifyUpdateCampaignVoucherSeries(campaign, campaignVoucherSeriesRecord, UserId);
                if (campaignVoucherSeries != null)
                {
                    _campaignService.UpdateCampaignVoucherSeries(campaign, campaignVoucherSeries, campaignVoucherSeriesRecord);
                    var campaignVoucherSeriesList = _campaignService.GetCampaignVoucherSeriesList(campaign);
                    return Ok(new ResponseMessage { Success = true, Message = "Update Campaign Voucher Series successfuly.", Data = new { CampaignVoucherSeriesList = campaignVoucherSeriesList } });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Campaign Voucher Series does not exist." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }

        // DELETE: <CampaignController>/Voucher/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28/8FF30B2C-F495-4542-8C4B-5F74B680B568
        [HttpDelete("Voucher/{campaignId:Guid}/{VoucherSeriesId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult DeleteCampaignVoucherSeries2(Guid campaignId, Guid voucherSeriesId)
        {
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                var campaignVoucherSeries = _campaignService.VerifyDeleteCampaignVoucherSeries(campaign, voucherSeriesId, UserId);
                if (campaignVoucherSeries != null)
                {
                    _campaignService.RemoveCampaignVoucherSeries(campaign, campaignVoucherSeries!);
                    var campaignVoucherSeriesList = _campaignService.GetCampaignVoucherSeriesList(campaign);
                    return Ok(new ResponseMessage { Success = true, Message = "Remove Campaign Voucher Series successfuly.", Data = new { CampaignVoucherSeriesList = campaignVoucherSeriesList } });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid delete request." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign does not exist." });
        }

        #region For end user

        // GET: <CampaignController>/CanJoin/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpGet("CanJoin/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "EndUser")]
        public IActionResult CheckUserCanJoin(Guid campaignId)
        {
            var endUser = _endUserService.GetById(UserId);
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null && endUser != null)
            {
                var canJoin = _campaignService.CheckUserCanJoin(campaign, endUser);
                if (canJoin)
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Enjoy now.", Data = new { CanJoin = true} });
                }
                return Ok(new ResponseMessage { Success = true, Message = "Can't join.", Data = new { CanJoin = false} });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Campaign or User does not exist." });
        }
        #endregion
    }
}
