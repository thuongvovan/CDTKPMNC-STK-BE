using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.Utilities.Email;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities.Validator;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IEmailService _mailler;
        private readonly JwtAuthen _jwtAuthen;
        private readonly IAccountAdminRepository _adminAccountRepository;
        public AdminController(IUnitOfWork unitOfWork, JwtAuthen jwtAuthen) // IEmailService mailler
        {
            _unitOfWork = unitOfWork;
            _adminAccountRepository = _unitOfWork.AccountAdminRepo;
            // _mailler = mailler;
            _jwtAuthen = jwtAuthen;
        }
        #region Sign-in_Sign-up
        // POST /<AdminController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginInfo account)
        {
            var adminAccount = _adminAccountRepository.GetByUserName(account.UserName);
            if (adminAccount != null && adminAccount.Password == account.Password.ToHashSHA256() && adminAccount.IsVerified)
            {
                var accountToken = _jwtAuthen.GenerateUserToken(adminAccount.Id, AccountType.Admin);
                var userToken = new AccountToken
                {
                    AccessToken = accountToken.AccessToken,
                    RefreshToken = accountToken.RefreshToken
                };
                if (adminAccount.AccountToken == null)
                {
                    adminAccount.AccountToken = userToken;
                }
                else
                {
                    adminAccount.AccountToken.AccessToken = userToken.AccessToken;
                    adminAccount.AccountToken.RefreshToken = userToken.RefreshToken;
                }
                _adminAccountRepository.Update(adminAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Login successfull.", Data = adminAccount });

            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Username or password is incorrect" });
        }

        // POST /<AdminController>/RefreshToken
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes = "AdminNoLifetime")]
        public IActionResult Refresh([FromBody] RefreshToken refreshToken)
        {
            var currentToken = new AccountToken
            {
                RefreshToken = refreshToken.Token,
                AccessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };

            var userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            AccountAdmin? account = _unitOfWork.AccountAdminRepo.GetById(userId!.Value);
            if (account != null && account.AccountToken!.AccessToken == currentToken.AccessToken && account.AccountToken.RefreshToken == currentToken.RefreshToken)
            {
                AccountToken userToken = _jwtAuthen.GenerateUserToken(account.Id, AccountType.Admin);
                account!.AccountToken!.AccessToken = userToken.AccessToken;
                account.AccountToken.RefreshToken = userToken.RefreshToken;
                _unitOfWork.AccountAdminRepo.Update(account);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "token refresh successful", Data = userToken });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid Refresh AccountToken." });
        }

        // PUT /<UserController>/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult ChangePassword(ChangePasswordInfo changePasswordAccount)
        {
            var validator = new ChangePasswordValidator();
            ValidationResult? validateResult;
            try
            {
                validateResult = validator.Validate(changePasswordAccount);
            }
            catch (Exception)
            {

                return BadRequest(new ResponseMessage { Success = false, Message = "Unable to verify data" });
            }

            if (!validateResult.IsValid)
            {
                string? ErrorMessage = validateResult.Errors?.FirstOrDefault()?.ErrorMessage;
                return BadRequest(new ResponseMessage { Success = false, Message = ErrorMessage! });
            }
            Guid? userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            AccountAdmin? currAccount = _unitOfWork.AccountAdminRepo.GetById(userId!.Value);
            if (currAccount != null && currAccount.Password == changePasswordAccount.OldPassword.ToHashSHA256() && !currAccount.IsVerified)
            {
                currAccount.Password = changePasswordAccount.NewPassword.ToHashSHA256();
                _unitOfWork.AccountAdminRepo.Update(currAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Change password successful" });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Your current password is incorrect." });
        }
        #endregion

        #region Partner
        // GET /<UserController>/Partner/All
        [HttpGet("Partner/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllPartners()
        {
            var partners = _unitOfWork.AccountPartnerRepo.GetAll();
            return Ok(new ResponseMessage { Success = true, Message = "Get the list of all partners successfully.", Data = new { Partners = partners } }); 
        }

        // GET /<UserController>/Partner/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpGet("Partner/{partnerId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetPartner(Guid partnerId)
        {
            var partner = _unitOfWork.AccountEndUserRepo.GetById(partnerId);
            if (partner != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get partner detail successful.", Data = new { EndUsers = partner } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "partnerId is not valid." });
        }
        #endregion

        #region Store
        // GET /<UserController>/Store/All
        [HttpGet("Store/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllStores()
        {
            var stores = _unitOfWork.StoreRepo.GetAll();
            if (stores != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of all stores successfully.", Data = new { Stores = stores } });
            }
            stores = new List<Store>();
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { Stores = stores } });
            
        }

        // GET /<UserController>/Store/Approved
        [HttpGet("Store/Approved")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetApprovedStores()
        {
            var stores = _unitOfWork.StoreRepo.GetApproved();
            if (stores != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of need approval stores successfully.", Data = new { Stores = stores } });
            }
            stores = new List<Store>();
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { Stores = stores } });
        }

        // GET /<UserController>/Store/Rejected
        [HttpGet("Store/Rejected")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetRejectedStores()
        {
            var stores = _unitOfWork.StoreRepo.GetRejected();
            if (stores != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of rejected stores successfully.", Data = new { Stores = stores } });
            }
            stores = new List<Store>();
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { Stores = stores } });
        }

        // GET /<UserController>/Store/NeedApproval
        [HttpGet("Store/NeedApproval")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetNeedApprovalStores()
        {
            var stores = _unitOfWork.StoreRepo.GetNeedApproval();
            if (stores != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of need approval stores successfully.", Data = new { Stores = stores } });
            }
            stores = new List<Store>();
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { Stores = stores } });
        }

        // PUT /<UserController>/Store/Approve/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Approve/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult ApproveStore(Guid storeId)
        {
            var store = _unitOfWork.StoreRepo.GetById(storeId);
            if (store != null)
            {
                _unitOfWork.StoreRepo.Approve(store);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Approved store successfully.", Data = new { Store = store } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "storeId is not valid." });
        }

        // PUT /<UserController>/Store/Reject/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Reject/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult RejectStore(Guid storeId)
        {
            var store = _unitOfWork.StoreRepo.GetById(storeId);
            if (store != null)
            {
                _unitOfWork.StoreRepo.Reject(store);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Reject store successfully.", Data = new { Store = store } });

            }
            return BadRequest(new ResponseMessage { Success = false, Message = "storeId is not valid." });
        }
        #endregion

        #region EndUser
        // GET /<UserController>/EndUser/All
        [HttpGet("EndUser/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllEndUser()
        {
            var endUsers = _unitOfWork.AccountEndUserRepo.GetAll();
            if (endUsers != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of end users successful.", Data = new { EndUsers = endUsers } });
            }
            endUsers = new List<AccountEndUser>();
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { EndUsers = endUsers } });
        }

        // GET /<UserController>/EndUser/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpGet("EndUser/{endUserId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetEndUser(Guid endUserId)
        {
            var endUser = _unitOfWork.AccountEndUserRepo.GetById(endUserId);
            if (endUser != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get end user detail successful.", Data = new { EndUsers = endUser } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "EndUserId is not valid." });
        }
        #endregion
    }
}
