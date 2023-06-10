using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NoticationController : CommonController
    {
        private readonly NoticationService _noticationService;
        private readonly AccountService<Account> _accountService;
        private readonly IDistributedCache _cache;
        public NoticationController(NoticationService noticationService, AccountService<Account> accountService, IDistributedCache cache)
        {
            _noticationService = noticationService;
            _accountService = accountService;
            _cache = cache;
        }

        // GET /<NoticationController>/Notication
        [HttpGet()]
        [Authorize(AuthenticationSchemes = "Account")]
        public async Task<IActionResult> GetNotications()
        {

            var cacheId = $"{UserId}_NoticationList";
            var notications = await _cache.GetRecordAsync<NoticaionsReturn>(cacheId);
            if (notications != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get notications successful.", Data = new { Notications = notications } });
            }

            var account = _accountService.GetById(UserId);
            if (account != null)
            {
                notications = _noticationService.GetNoticationByUser(UserId);
                await _cache.SetRecordAsync(cacheId, notications, TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(1)).ConfigureAwait(false);
                return Ok(new ResponseMessage { Success = true, Message = "Get notications successful.", Data = new { Notications = notications } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid UserId." });
        }

        // PUT /<NoticationController>/Notication/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("{noticationId}")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult MarkAsRead(Guid noticationId)
        {
            var notication = _noticationService.GetById(noticationId);
            if (notication != null && notication.AccountId == UserId)
            {
                _noticationService.MarkAsRead(notication);
                var cacheId = $"{UserId}_NoticationList";
                _cache.RemoveAsync(cacheId);
                return Ok(new ResponseMessage { Success = true, Message = "Notification has been marked as read.", Data = new { Notication = notication } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }

    }
}
