using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NoticationController : CommonController
    {
        private readonly NoticationService _noticationService;
        private readonly AccountService<Account> _accountService;
        public NoticationController(NoticationService noticationService, AccountService<Account> accountService)
        {
            _noticationService = noticationService;
            _accountService = accountService;
        }

        // GET /<NoticationController>/Notication
        [HttpGet()]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetNotications()
        {
            var account = _accountService.GetById(UserId);
            if (account != null)
            {
                var notications = _noticationService.GetNoticationByUser(UserId);
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
                return Ok(new ResponseMessage { Success = true, Message = "Notification has been marked as read.", Data = new { Notication = notication } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }

    }
}
