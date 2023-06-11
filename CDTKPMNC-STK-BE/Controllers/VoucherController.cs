using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Crypto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VoucherController : CommonController
    {
        private readonly VoucherService _voucherService;
        private readonly StoreService _storeService;
        private readonly EndUserService _endUserService;
        private readonly IDistributedCache _cache;
        public VoucherController(VoucherService voucherService, StoreService storeService, EndUserService endUserService, IDistributedCache cache)
        {
            _voucherService = voucherService;
            _storeService = storeService;
            _endUserService = endUserService;
            _cache = cache;
        }

        // GET: /<VoucherController>/VoucherSeries/All
        [HttpGet("VoucherSeries/All")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetListVoucherSeries()
        {
            if (UserType == AccountType.Admin)
            {
                var voucherSeriesListAll = _voucherService.GetListVoucherSeries();
                return Ok(new ResponseMessage { Success = true, Message = "Get all voucher series successful.", Data = new { VoucherSeriesList = voucherSeriesListAll } });
            }
            else if (UserType == AccountType.Partner)
            {
                var store = _storeService.GetById(UserId);
                if (store != null)
                {
                    var voucherSeriesList = _voucherService.GetListVoucherSeries(UserId);
                    return Ok(new ResponseMessage { Success = true, Message = "Get all voucher series successful.", Data = new { VoucherSeriesList = voucherSeriesList } });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Store dose not exist" });
            }
            return BadRequest();
        }

        // POST: /<VoucherController>/VoucherSeries/All/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpGet("VoucherSeries/All/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetListVoucherSeries(Guid storeId)
        {
            var store = _storeService.GetById(UserId);
            if (store != null)
            {
                var voucherSeriesList = _voucherService.GetListVoucherSeries(storeId);
                if (UserType == AccountType.Admin || (UserType == AccountType.Partner && UserId == storeId) )
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Get all voucher series by store successful.", Data = new { VoucherSeriesList = voucherSeriesList } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store dose not exist" });
        }

        // POST: /<VoucherController>/VoucherSeries/Create
        [HttpPost("VoucherSeries/Create")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult CreateVoucherSeries(VoucherSeriesRecord voucherSeriesRecord)
        {
            var validateSummary = _voucherService.ValidateVoucherSeriesRecord(voucherSeriesRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var store = _storeService.GetById(UserId);
            if (store != null)
            {
                var isVerified = _voucherService.VerifyVoucherSeriesRecord(store, voucherSeriesRecord);
                if (isVerified)
                {
                    var voucherSeries = _voucherService.AddVoucherSeries(store, voucherSeriesRecord);
                    return Ok(new ResponseMessage { Success = true, Message = "Create a successful voucher series.", Data = new { VoucherSeries = voucherSeries } });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Voucher series is really existed" });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store dose not exist" });
        }

        // GET: /<VoucherController>/VoucherSeries/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpGet("VoucherSeries/{voucherSeriesId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetVoucherSeries(Guid voucherSeriesId)
        {
            var voucherSeries = _voucherService.GetVoucherSeries(voucherSeriesId);
            if (voucherSeries != null)
            {
                if (UserType == AccountType.Admin || (UserType == AccountType.Partner && voucherSeries.StoreId == UserId))
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Get voucher series successful.", Data = new { VoucherSeries = voucherSeries } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Voucher Series dose not exist" });
        }

        // PUT: /<VoucherController>/VoucherSeries/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpPut("VoucherSeries/{voucherSeriesId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult UpdateVoucherSeries(Guid voucherSeriesId, [FromBody] VoucherSeriesRecord voucherSeriesRecord)
        {
            var validateSummary = _voucherService.ValidateVoucherSeriesRecord(voucherSeriesRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var store = _storeService.GetById(UserId);
            var voucherSeries = _voucherService.GetVoucherSeries(voucherSeriesId);
            if (store != null && voucherSeries != null)
            {
                var isVerified = _voucherService.VerifyUpdateVoucherSeries(store, voucherSeries, voucherSeriesRecord);
                if (isVerified)
                {
                    _voucherService.UpdateVoucherSeries(voucherSeries, voucherSeriesRecord);
                    return Ok(new ResponseMessage { Success = true, Message = "Voucher series has been updated.", Data = new { VoucherSeries = voucherSeries } });
                }     
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Voucher series dose not exist." });
        }

        // DELETE: /<VoucherController>/VoucherSeries/34F6BF30-5F84-4B93-B5BD-08DB5A09AE28
        [HttpDelete("VoucherSeries/{voucherSeriesId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult DeleteVoucherSeries(Guid voucherSeriesId)
        {
            var store = _storeService.GetById(UserId);
            if (store != null)
            {
                var voucherSeries = store.VoucherSeries.SingleOrDefault(vs => vs.Id == voucherSeriesId);
                if (voucherSeries != null)
                {
                    var isVerified = _voucherService.VerifyDeleteVoucherSeries(voucherSeries);
                    if (isVerified)
                    {
                        _voucherService.DeleteVoucherSeries(voucherSeries);
                        return Ok(new ResponseMessage { Success = true, Message = "Voucher series has been deleted." });
                    }
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Voucher Series dose not exist" });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store dose not exist" });
        }

        // GET: /<VoucherController>/All
        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetVouchers()
        {
            if (UserType == AccountType.Admin)
            {
                var vouchers = _voucherService.GetVoucherAll();
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get", Data = new { Vouchers = vouchers } });

            }
            else if (UserType == AccountType.Partner)
            {
                var vouchers = _voucherService.GetVoucherPartner(UserId);
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get", Data = new { Vouchers = vouchers } });
            }
            else if (UserType == AccountType.EndUser)
            {
                var vouchers = _voucherService.GetVoucherEndUser(UserId);
                return Ok(new ResponseMessage { Success = true, Message = "Successfuly get", Data = new { Vouchers = vouchers } });
            }
            return BadRequest();
        }

        // GET: /<VoucherController>/10D87262-0C2C-430E-9CC2-D4426D33F888
        [HttpGet("{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetVouchers(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                if (UserType == AccountType.Admin || (UserType == AccountType.Partner && storeId == UserId))
                { 
                    var vouchers = _voucherService.GetVoucherStore(store);
                    return Ok(new ResponseMessage { Success = true, Message = "Successfuly get", Data = new { Vouchers = vouchers } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store dose not exist" });
        }

        // PUT: /<VoucherController>/Share
        [HttpPut("Share")]
        [Authorize(AuthenticationSchemes = "EndUser")]
        public IActionResult ShareVouchers(VoucherShareRecord voucherShareRecord)
        {
            var validateSummary = _voucherService.ValidateVoucherShareRecord(voucherShareRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var recipientUser = _endUserService.GetByUserName(voucherShareRecord.DestinationUser!);
            if (recipientUser == null)
            {
                return BadRequest(new ResponseMessage(false, "Recipient does not exist"));
            }
            var endUser = _endUserService.GetById(UserId);
            if (endUser == null)
            {
                return BadRequest(new ResponseMessage(false, "User does not exist"));
            }
            if (endUser.Id == recipientUser.Id)
            {
                return BadRequest(new ResponseMessage(false, "Invalid request."));
            }
            var voucher = _voucherService.GetVoucher(voucherShareRecord.VoucherCode!.Value);
            if (voucher != null && voucher.EndUserId == UserId && !voucher.IsUsed && voucher.CampaignVoucherSeries.ExpiresOn.ToDateTime() >=  DateTime.Now)
            {
                _cache.RemoveAsync($"{UserId}_NoticationList");
                _cache.RemoveAsync($"{recipientUser.Id}_NoticationList");
                _voucherService.ChangeVoucherOwner(recipientUser, voucher);
                _voucherService.SendEmailShareVoucher(endUser, recipientUser, voucher);
                _voucherService.SendNoticationShareVoucher(endUser, recipientUser, voucher);
                return Ok(new ResponseMessage { Success = true, Message = "Share voucher Successfuly"});
            }
            return BadRequest(new ResponseMessage(false, "The voucher is not valid for use or has expired."));
        }

        // GET: /<VoucherController>/DeleteAll
        [HttpGet("DeleteAll")]
        public IActionResult DeleteAllVouchers()
        {
            _voucherService.DeleteAllVouchers();
            return Ok();
        }    
    }
}
