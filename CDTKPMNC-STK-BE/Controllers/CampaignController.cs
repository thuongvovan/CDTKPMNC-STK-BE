using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CampaignController : CommonController
    {
        private readonly CampaignService _campaignService;
        private readonly StoreService _storeService;
        public CampaignController(CampaignService campaignService, StoreService storeService)
        {
            _campaignService = campaignService;
            _storeService = storeService;
        }

        // GET: <CampaignController>/All
        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetListCampaign()
        {
            if (UserType == AccountType.Admin)
            {
                var listCampaignAll = _campaignService.GetListCampaign();
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all campaign.", Data = new { ListCampaign = listCampaignAll } });
            }
            else if (UserType == AccountType.Partner)
            {
                var listCampaign = _campaignService.GetListCampaign(UserId);
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get all your store campaign.", Data = new { ListCampaign = listCampaign } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }

        // GET: <CampaignController>/All
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
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
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

        // PUT: <CampaignController>/Info/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPut("Info/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
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

        // POST: <CampaignController>/Voucher/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPost("Voucher/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
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
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
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

        // DELETE: <CampaignController>/Voucher/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpDelete("Voucher/{campaignId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult DeleteCampaignVoucherSeries(Guid campaignId, [FromBody] VoucherSeriesDeleteRecord voucher)
        {
            var campaign = _campaignService.GetCampaign(campaignId);
            if (campaign != null)
            {
                var campaignVoucherSeries = _campaignService.VerifyDeleteCampaignVoucherSeries(campaign, voucher.VoucherSeriesId, UserId);
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
    }
}
