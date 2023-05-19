﻿using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices;
using Org.BouncyCastle.Tls;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AdminController : CommonController
    {
        private readonly AdminService _adminService;
        private readonly StoreService _storeService;
        private readonly PartnerService _partnerService;
        private readonly EndUserService _endUserService;
        public AdminController(AdminService adminService, PartnerService partnerService, EndUserService endUserService, StoreService storeService)
        {
            _adminService = adminService;
            _storeService = storeService;
            _partnerService = partnerService;
            _endUserService = endUserService;
        }
        #region Sign-in_Sign-up
        // POST /<AdminController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRecord loginRecord)
        {
            var validateSummary = _adminService.ValidateLoginRecord(loginRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false,validateSummary.ErrorMessage));
            }
            var accountAdmin = _adminService.GetByUserName(loginRecord!.UserName!);
            var isLoginSuccess = _adminService.VerifyLogin(accountAdmin, loginRecord);
            if (isLoginSuccess)
            {
                _adminService.GenerateToken(accountAdmin!, AccountType.Admin);
                return Ok(new ResponseMessage(true, "Login successfully.", new { Account = accountAdmin, Token = accountAdmin!.AccountToken }));
            }
            return BadRequest(new ResponseMessage(false, "Username or password is incorrect"));
        }

        // POST /<AdminController>/RefreshToken
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes = "AdminNoLifetime")]
        public IActionResult Refresh([FromBody] TokenRecord tokenRecord)
        {
            var receieToken = new AccountToken
            {
                RefreshToken = tokenRecord.Token,
                AccessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };
            
            var accountAdmin = _adminService.GetById(UserId);
            var isValid = _adminService.VerifyRefreshToken(accountAdmin!, receieToken, UserType);
            if (isValid)
            {
                _adminService.GenerateToken(accountAdmin!, UserType);
                return Ok(new ResponseMessage(true, "Refresh token successfully.", accountAdmin!.AccountToken));
            }
            return Ok(new ResponseMessage(false, "Your token is invalid."));
        }

        // PUT /<UserController>/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult ChangePassword(ChangePasswordRecord changePasswordRecord)
        {
            var validateSummary = _adminService.ValidateChangePasswordRecord(changePasswordRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var accountAdmin = _adminService.GetById(UserId);
            var isMatched = _adminService.VerifyCurrentPassword(accountAdmin!, changePasswordRecord);
            if (isMatched)
            {
                _adminService.ChangePassword(accountAdmin!, changePasswordRecord);
                return Ok(new ResponseMessage(true, "Change password successfully.", accountAdmin));
            }
            return Ok(new ResponseMessage(false, "Your current password is incorrect."));
        }

        // PUT /<UserController>/Update
        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult UpdateAccount(AdminUpdateRecord adminUpdateRecord)
        {
            var validateSummary = _adminService.ValidateAdminUpdateRecord(adminUpdateRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var accountAdmin = _adminService.GetById(UserId);
            if (accountAdmin != null)
            {
                _adminService.UpdateAccount(accountAdmin, adminUpdateRecord);
                return Ok(new ResponseMessage(true, "Update successfully.", accountAdmin));
            }
            return Ok(new ResponseMessage(false, "An error occurred while updating."));
        }
        #endregion

        #region Partner
        // GET /<UserController>/Partner/All
        [HttpGet("Partner/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllPartners()
        {
            var partners = _partnerService.GetAll();
            if (partners.Count > 0)
            {
                return Ok(new ResponseMessage(true, "Get the list of all partners successfully.",new { Partners = partners }));
            }
            return Ok(new ResponseMessage(true, "The list is empty.", new { Partners = partners }));
        }

        // GET /<UserController>/Partner/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpGet("Partner/{partnerId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetPartner(Guid partnerId)
        {
            var partner = _partnerService.GetById(partnerId);
            if (partner != null)
            {
                return Ok(new ResponseMessage(true, "Get partner detail successfully.", new { Partner = partner } ));
            }
            return BadRequest(new ResponseMessage(false, "endUserId is not valid." ));
        }

        // PUT /<UserController>/Partner/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Partner/{partnerId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult UpdatePartner(Guid partnerId, [FromBody] PartnerUpdateRecord partnerUpdateRecord)
        {
            var validateResult = _partnerService.ValidatePartnerUpdateRecord(partnerUpdateRecord);
            if (!validateResult.IsValid)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = validateResult.ErrorMessage });
            }
            var partner = _partnerService.GetById(partnerId);
            if (partner != null)
            {
                partner = _partnerService.UpdateAccount(partner, partnerUpdateRecord);
                return Ok(new ResponseMessage(true, "Update successfully.", new { Partner = partner }));
            }
            return BadRequest(new ResponseMessage(false, "endUserId is not valid."));
        }

        #endregion

        #region Store
        // GET /<UserController>/Store/All
        [HttpGet("Store/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllStores()
        {
            var stores = _storeService.GetAll();
            if (stores.Count > 0)
            {
                return Ok(new ResponseMessage (true, "Get the list of all stores successfully.", new { Stores = stores } ));
            }
            return Ok(new ResponseMessage(true, "List of store is empty.", new { Stores = stores }));
        }

        // GET /<UserController>/Store/Approved
        [HttpGet("Store/Approved")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetApprovedStores()
        {
            var stores = _storeService.GetApproved();
            if (stores.Count > 0)
            {
                return Ok(new ResponseMessage(true, "Get the list of need approval stores successfully.", new { Stores = stores }));
            }
            return Ok(new ResponseMessage(true, "List of store is empty.", new { Stores = stores }));
        }

        // GET /<UserController>/Store/Rejected
        [HttpGet("Store/Rejected")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetRejectedStores()
        {
            var stores = _storeService.GetRejected();
            if (stores.Count > 0)
            {
                return Ok(new ResponseMessage(true, "Get the list of rejected stores successfully.", new { Stores = stores }));
            }
            return Ok(new ResponseMessage(true, "List of store is empty.", new { Stores = stores }));
        }

        // GET /<UserController>/Store/NeedApproval
        [HttpGet("Store/NeedApproval")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetNeedApprovalStores()
        {
            var stores = _storeService.GetNeedApproval();
            if (stores.Count > 0)
            {
                return Ok(new ResponseMessage(true, "Get the list of need approval stores successfully.", new { Stores = stores }));
            }
            return Ok(new ResponseMessage(true, "List of store is empty.", new { Stores = stores }));
        }

        // PUT /<UserController>/Store/Approve/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Approve/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult ApproveStore(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                _storeService.Approve(store);
                return Ok(new ResponseMessage(true, "Approved store successfully.",new { Store = store } ));
            }
            return BadRequest(new ResponseMessage(false,"storeId is not valid."));
        }

        // PUT /<UserController>/Store/Reject/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Reject/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult RejectStore(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                _storeService.Reject(store);
                return Ok(new ResponseMessage(true, "Reject store successfully.", new { Store = store }));
            }
            return BadRequest(new ResponseMessage(false, "storeId is not valid."));
        }

        // PUT /<UserController>/Store/Enable/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Enable/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult EnableStore(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                _storeService.Enable(store);
                return Ok(new ResponseMessage(true, "Enable store successfully.", new { Store = store }));
            }
            return BadRequest(new ResponseMessage(false, "storeId is not valid."));
        }

        // PUT /<UserController>/Store/Disable/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Disable/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DisableStore(Guid storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store != null)
            {
                _storeService.Disable(store);
                return Ok(new ResponseMessage(true, "Disable store successfully.", new { Store = store }));
            }
            return BadRequest(new ResponseMessage(false, "storeId is not valid."));
        }
        #endregion

        #region EndUser
        // GET /<UserController>/EndUser/All
        [HttpGet("EndUser/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllEndUser()
        {
            var endUsers = _endUserService.GetAll();
            if (endUsers.Count > 0)
            {
                return Ok(new ResponseMessage(true, "Get the list of all partners successfully.", new { Partners = endUsers }));
            }
            return Ok(new ResponseMessage(true, "The list is empty.", new { EndUsers = endUsers }));
        }

        // GET /<UserController>/EndUser/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpGet("EndUser/{endUserId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetEndUser(Guid endUserId)
        {
            var endUser = _endUserService.GetById(endUserId);
            if (endUser != null)
            {
                return Ok(new ResponseMessage(true, "Get end user detail successfully.", new { EndUsers = endUser } ));
            }
            return BadRequest(new ResponseMessage(true, "EndUserId is not valid."));
        }

        // PUT /<UserController>/EndUser/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("EndUser/{endUserId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult UpdateEndUser(Guid endUserId, [FromBody] AccountUpdateRecord accountUpdateRecord)
        {
            var validateResult = _endUserService.ValidateAccountUpdateRecord(accountUpdateRecord);
            if (!validateResult.IsValid)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = validateResult.ErrorMessage });
            }
            var endUser = _endUserService.GetById(endUserId);
            if (endUser != null)
            {
                endUser = _endUserService.UpdateAccount(endUser, accountUpdateRecord);
                return Ok(new ResponseMessage(true, "Update successfully.", new { EndUser = endUser }));
            }
            return BadRequest(new ResponseMessage(false, "endUserId is not valid."));
        }

        #endregion
    }
}