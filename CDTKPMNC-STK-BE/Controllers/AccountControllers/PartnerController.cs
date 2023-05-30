using CDTKPMNC_STK_BE.Utilities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PartnerController : CommonController
    {
        private readonly PartnerService _partnerService;
        private readonly OtpService _otpService;
        private readonly StoreService _storeService;
        private readonly GameService _gameService;
        public PartnerController(PartnerService partnerService, StoreService storeService, OtpService otpService, GameService gameService)
        {
            _partnerService = partnerService;
            _otpService = otpService;
            _storeService = storeService;
            _gameService = gameService;
        }

        #region account
        // POST /<PartnerController>/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] PartnerRegistrationRecord partnerRegistrationRecord)
        {
            // ============== Tai khoan test ===============
            if (partnerRegistrationRecord != null &&
                partnerRegistrationRecord.Account!.UserName != null &&
                partnerRegistrationRecord.Account.UserName.StartsWith("testpartner@"))
            {
                AccountPartner? currAccountTest = _partnerService.GetByUserName(partnerRegistrationRecord.Account.UserName);
                if (currAccountTest != null)
                {
                    if (currAccountTest.IsVerified)
                    {
                        return Conflict(new ResponseMessage(false, "UserName already exists."));
                    }
                    _partnerService.DeleteAccount(currAccountTest);
                }
                var testAccountPartner = new AccountPartner
                {
                    UserName = partnerRegistrationRecord!.Account!.UserName,
                    Name = "Tài Khoản Test Partner",
                    Password = "123456".ToHashSHA256(),
                    Otp = new AccountOtp { RegisterOtp = 123456, RegisterExpiresOn = DateTime.Now.AddHours(2) },
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                    Gender = Gender.Others,
                    Address = new Address
                    {
                        Street = "Đường Test",
                        WardId = "26734"
                    },
                    CreatedAt = DateTime.Now
                };
                _partnerService.AddAccount(testAccountPartner);
                return Ok(new ResponseMessage(true, "Test partner loginRecord Otp & Password is: 123456", new { Account = testAccountPartner }));
            }

            // ============== Tai khoan thuong =============
            var validateSummary = _partnerService.ValidatePartnerRegistrationRecord(partnerRegistrationRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            AccountPartner? currAccount = _partnerService.GetByUserName(partnerRegistrationRecord!.Account!.UserName!);
            if (currAccount != null)
            {
                if (currAccount.IsVerified)
                {
                    return Conflict(new ResponseMessage { Success = false, Message = "UserName already exists." });
                }
                _partnerService.DeleteAccount(currAccount);
            }
            var accountPartner = _partnerService.CreateAccount(partnerRegistrationRecord);
            _otpService.GenerateRegisterOtp(accountPartner);
            _otpService.SendRegisterOTPPartner(accountPartner);
            // _emailService.SendRegisterOTP(accountPartner);
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "Next step, verify your loginRecord in 10 minutes.",
                Data = new { UserAccount = accountPartner }
            });
        }

        // GET /<PartnerController>/Info
        [HttpGet("Info")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult Update()
        {
            AccountPartner? accountPartner = _partnerService.GetById(UserId);
            if (accountPartner != null)
            {
                return Ok(new ResponseMessage
                {
                    Success = true,
                    Message = "Successfully get.",
                    Data = new { Account = accountPartner }
                });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid UserId." });
        }

        // PUT /<PartnerController>/Update
        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult Update([FromBody] PartnerUpdateRecord partnerUpdateRecord)
        {
            AccountPartner? accountPartner = _partnerService.GetById(UserId);
            if (accountPartner != null)
            {
                var validateResult = _partnerService.ValidatePartnerUpdateRecord(accountPartner, partnerUpdateRecord);
                if (!validateResult.IsValid)
                {
                    return BadRequest(new ResponseMessage { Success = false, Message = validateResult.ErrorMessage });
                }
                accountPartner = _partnerService.UpdateAccount(accountPartner, partnerUpdateRecord);
                return Ok(new ResponseMessage
                {
                    Success = true,
                    Message = "Update successfully.",
                    Data = new { Account = accountPartner }
                });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid UserId." });
        }

        // POST /<PartnerController>/VerifyRegister/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("VerifyRegister/{userId:Guid}")]
        public IActionResult VerifyRegister(Guid userId, [FromBody] Otp otp)
        {
            if (otp == null || otp.OtpValue == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid OTP." });
            }
            AccountPartner? accountPartner = _partnerService.GetById(userId);
            if (accountPartner != null)
            {
                var isVerified = _otpService.Verify(accountPartner, otp);
                if (isVerified)
                {
                    _partnerService.GenerateToken(accountPartner!, AccountType.Partner);
                    return Ok(new ResponseMessage
                    {
                        Success = true,
                        Message = "Successfully verified.",
                        Data = new { Account = accountPartner, Token = accountPartner!.AccountToken }
                    });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid OTP." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid UserId." });
        }

        // POST /<PartnerController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRecord loginRecord)
        {
            var validateSummary = _partnerService.ValidateLoginRecord(loginRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            AccountPartner? accountPartner = _partnerService.GetByUserName(loginRecord!.UserName!);
            var isLoginSuccess = _partnerService.VerifyLogin(accountPartner, loginRecord);
            if (isLoginSuccess)
            {
                _partnerService.GenerateToken(accountPartner!, AccountType.Partner);
                return Ok(new ResponseMessage(true, "Login successfully.", new { Account = accountPartner, Token = accountPartner!.AccountToken }));
            }
            return BadRequest(new ResponseMessage(false, "Username or password is incorrect"));
        }

        // POST /<PartnerController>/RefreshToken
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes = "PartnerNoLifetime")]
        public IActionResult RefreshToken([FromBody] TokenRecord tokenRecord)
        {
            var receieToken = new AccountToken
            {
                RefreshToken = tokenRecord.Token,
                AccessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };

            var accountPartner = _partnerService.GetById(UserId);
            var isValid = _partnerService.VerifyRefreshToken(accountPartner!, receieToken, UserType);
            if (isValid)
            {
                _partnerService.GenerateToken(accountPartner!, UserType);
                return Ok(new ResponseMessage(true, "Refresh token successfully.", accountPartner!.AccountToken));
            }
            return Ok(new ResponseMessage(false, "Your token is invalid."));
        }

        // PUT /<PartnerController>/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult ChangePassword(ChangePasswordRecord changePasswordRecord)
        {
            var validateSummary = _partnerService.ValidateChangePasswordRecord(changePasswordRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var accountPartner = _partnerService.GetById(UserId);
            var isMatched = _partnerService.VerifyCurrentPassword(accountPartner!, changePasswordRecord);
            if (isMatched)
            {
                _partnerService.ChangePassword(accountPartner!, changePasswordRecord);
                return Ok(new ResponseMessage(true, "Change password successfully.", accountPartner));
            }
            return BadRequest(new ResponseMessage(false, "Your current password is incorrect."));
        }

        // POST /<PartnerController>/ResetPassword
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordRecord resetPasswordRecord)
        {
            var validateSummary = _partnerService.ValidateResetPasswordRecord(resetPasswordRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));

            }
            var accountPartner = _partnerService.GetByUserName(resetPasswordRecord.UserName!);
            if (accountPartner != null && accountPartner.IsVerified)
            {
                _partnerService.SetNewPasswordPending(accountPartner, resetPasswordRecord);
                _otpService.GenerateResetPasswordOtp(accountPartner);
                _otpService.SendResetPasswordOTP(accountPartner);
                return Ok(new ResponseMessage { Success = true, Message = "Please check your email to verify this change." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "UserName does not exist" });
        }

        // POST /<PartnerController>/VerifyResetPassword
        [HttpPost("VerifyResetPassword")]
        public IActionResult VerifyResetPassword([FromBody] VerifyResetPasswordRecord verifyResetRecord)
        {
            var validateSummary = _partnerService.ValidateVerifyResetPasswordRecord(verifyResetRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));

            }
            var accountPartner = _partnerService.GetByUserName(verifyResetRecord.UserName!);
            var isMatched = _partnerService.VerifyResetPassword(accountPartner!, verifyResetRecord);
            if (isMatched)
            {
                _partnerService.ApproveNewPassword(accountPartner!);
                _partnerService.GenerateToken(accountPartner!, UserType);
                return Ok(new ResponseMessage(true, "Change password successfully.", new { Account = accountPartner, Token = accountPartner!.AccountToken }));
            }
            return BadRequest(new ResponseMessage(false, "Incorrect OTP."));
        }
        #endregion

        #region store
        // POST /<PartnerController>/Store/Register
        [HttpPost("Store/Register")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult RegisterStore([FromBody] StoreRecord storeRecord)
        {
            var validateSummary = _storeService.ValidateStoreRecord(storeRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var isVerified = _storeService.VerifyStoreRecord(storeRecord, UserId);
            if (isVerified)
            {
                var store = _storeService.AddStore(storeRecord, UserId);
                return Ok(new ResponseMessage { Success = true, Message = "Create a successful store. The request is being reviewed by the administrator", Data = new { Store = store } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store is really existed" });
        }

        // PUT /<PartnerController>/Store/Update
        [HttpPut("Store/Update")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult UpdateStore([FromBody] StoreRecord storeRecord)
        {
            var validateSummary = _storeService.ValidateStoreRecord(storeRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var store = _storeService.GetById(UserId);
            if (store != null)
            {
                _storeService.UpdateStore(store, storeRecord);
                return Ok(new ResponseMessage { Success = true, Message = "Store has been updated.", Data = new { Store = store } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store dose not exist." });
        }

        // GET /<PartnerController>/Store/Detail
        [HttpGet("Store/Detail")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult GetStore()
        {
            var store = _storeService.GetById(UserId);
            if (store != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get successfully.", Data = new { Store = store } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Store dose not exist." });
        }

        // PUT /<PartnerController>/Store/Enable
        [HttpPut("Store/Enable")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult EnableStore()
        {
            var store = _storeService.EnableStore(UserId);
            if (store != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Store has been enabled.", Data = new { Store = store } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Enable is not valid. Please register store." });
        }

        // PUT /<PartnerController>/Store/Disable
        [HttpPut("Store/Disable")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult DisableStore()
        {
            var store = _storeService.DisableStore(UserId);
            if (store != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Store has been disabled.", Data = new { Store = store } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Disable is not valid. Please register store." });
        }
        #endregion

        #region Game
        // GET /<PartnerController>/Game/All
        [HttpGet("Game/All")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult GetAllGame()
        {
            var games = _gameService.GetAllGame();
            if (games.Count > 0)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of game successful.", Data = new { Games = games } });
            }
            return Accepted(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { Games = games } });
        }

        // GET /<PartnerController>/Game/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpGet("Game/{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult GetGame(Guid gameId)
        {
            var game = _gameService.GetById(gameId);
            if (game != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get game detail successful.", Data = new { Game = game } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "gameId is not valid." });
        }

        #endregion
    }

}
