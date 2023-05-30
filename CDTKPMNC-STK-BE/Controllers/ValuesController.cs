using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: /<ValuesController>/Gender
        [HttpGet("Gender")]
        public IActionResult GetGender()
        {
            var genderValue = Enum.GetNames(typeof(Gender)).ToList();
            return Ok(new ResponseMessage 
                { 
                    Success = true, 
                    Message = "OK", 
                    Data = new { GenderValue = genderValue }
                });

        }

        // GET: /<ValuesController>/PartnerType
        [HttpGet("PartnerType")]
        public IActionResult GetPartnerType()
        {
            var partnerTypeValue = Enum.GetNames(typeof(PartnerType)).ToList();
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "OK",
                Data = new { PartnerTypValue = partnerTypeValue }
            });
        }

        // GET: /<ValuesController>/AccountType
        [HttpGet("AccountType")]
        public IActionResult GetUserType()
        {
            var userTypeValue = Enum.GetNames(typeof(AccountType)).ToList();
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "OK",
                Data = new { UserTypeValue = userTypeValue }
            });
        }

        // GET: /<ValuesController>/CampaignStatus
        [HttpGet("CampaignStatus")]
        public IActionResult GetCampaignStatus()
        {
            var campaignStatusValue = Enum.GetNames(typeof(CampaignStatus)).ToList();
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "OK",
                Data = new { CampaignStatusValue = campaignStatusValue }
            });
        }

        // GET: /<ValuesController>/GameRule
        [HttpGet("GameRule")]
        public IActionResult GetGameRule()
        {
            var gameRuleValue = Enum.GetNames(typeof(GameRule)).ToList();
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "OK",
                Data = new { GameRuleValue = gameRuleValue }
            });
        }
    }
}


