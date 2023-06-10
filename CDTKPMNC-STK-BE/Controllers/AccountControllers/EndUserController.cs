using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EndUserController : CommonController
    {
        // private readonly PartnerService _partnerService;
        private readonly EndUserService _endUserService;
        private readonly OtpService _otpService;
        private readonly GameService _gameService;
        private readonly StoreService _storeService;
        private readonly IDistributedCache _cache;
        public EndUserController(EndUserService endUserService, OtpService otpService, GameService gameService, StoreService storeService, IDistributedCache cache)
        {
            // _partnerService = partnerService;
            _endUserService = endUserService;
            _otpService = otpService;
            _gameService = gameService;
            _storeService = storeService;
            _cache = cache;
        }

        #region Account
        // POST /<EndUserController>/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] AccountRegistrationRecord userRegistrationRecord)
        {
            // ============== Tai khoan test =============
            if (userRegistrationRecord != null && 
                userRegistrationRecord!.UserName != null && 
                userRegistrationRecord.UserName.StartsWith("test@"))
            {
                AccountEndUser? currAccountTest = _endUserService.GetByUserName(userRegistrationRecord!.UserName);
                if (currAccountTest != null)
                {
                    if (currAccountTest.IsVerified)
                    {
                        return Conflict(new ResponseMessage { Success = false, Message = "UserName already exists." });
                    }
                    _endUserService.DeleteAccountEndUser(currAccountTest);
                }
                var testAccount = new AccountEndUser 
                    {
                        UserName = userRegistrationRecord.UserName,
                        Name = "Tài Khoản Test",
                        Password = "123456".ToHashSHA256(),
                        Otp = new AccountOtp { RegisterOtp = 123456, RegisterExpiresOn = DateTime.Now.AddHours(2) },
                        DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                        Gender = Gender.Others,
                        Address = new Address
                        {
                            Street = "Đường Test",
                            WardId = "26734"
                        },
                        CreatedAt = DateTime.Now,
                };
                _endUserService.AddAccountEndUser(testAccount);
                return Ok(new ResponseMessage 
                    { 
                        Success = true, 
                        Message = "Test userRegistrationRecord RegisterOtp & Password is: 123456", 
                        Data = new { Account = testAccount }
                    });
            }
            // ============== Tai khoan thuong =============
            var validateSummary = _endUserService.ValidateUserRegistrationRecord(userRegistrationRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            AccountEndUser? accountEndUser = _endUserService.GetByUserName(userRegistrationRecord!.UserName!);
            if (accountEndUser != null)
            {
                if (accountEndUser.IsVerified)
                {
                    return Conflict(new ResponseMessage { Success = false, Message = "UserName already exists." });
                }
                _endUserService.DeleteAccountEndUser(accountEndUser);
            }

            accountEndUser = _endUserService.CreateAccountEndUser(userRegistrationRecord);
            _otpService.GenerateRegisterOtp(accountEndUser);
            _otpService.SendRegisterOTPEndUser(accountEndUser);
            // _emailService.SendRegisterOTP(accountEndUser);
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "Next step, verify your loginRecord in 10 minutes.",
                Data = new { Account = accountEndUser }
            });
        }


        // POST /<EndUserController>/VerifyRegister/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("VerifyRegister/{userId:Guid}")]
        public IActionResult VerifyRegister(Guid userId, [FromBody] Otp otp)
        {
            if (otp == null || otp.OtpValue == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid OTP." });
            }
            AccountEndUser? accountEndUser = _endUserService.GetById(userId);
            if (accountEndUser != null)
            {
                var isVerified = _otpService.Verify(accountEndUser, otp);
                if (isVerified)
                {
                    _endUserService.GenerateToken(accountEndUser!, AccountType.EndUser);
                    return Ok(new ResponseMessage
                    {
                        Success = true,
                        Message = "Successfully verified.",
                        Data = new { Account = accountEndUser, Token = accountEndUser!.AccountToken }
                });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid OTP." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid UserId." });
        }

        // POST /<EndUserController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRecord loginRecord)
        {
            var validateSummary = _endUserService.ValidateLoginRecord(loginRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var accountEndUser = _endUserService.GetByUserName(loginRecord!.UserName!);
            var isLoginSuccess = _endUserService.VerifyLogin(accountEndUser, loginRecord);
            if (isLoginSuccess)
            {
                _endUserService.GenerateToken(accountEndUser!, AccountType.EndUser);
                return Ok(new ResponseMessage(true, "Login successfully.", new { Account = accountEndUser, Token = accountEndUser!.AccountToken }));
            }
            return BadRequest(new ResponseMessage(false, "Username or password is incorrect"));
        }

        // POST /<EndUserController>/RefreshToken
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes = "EndUserNoLifetime")]
        public IActionResult Refresh([FromBody] TokenRecord tokenRecord)
        {
            var receieToken = new AccountToken
            {
                RefreshToken = tokenRecord.Token,
                AccessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };

            var accountEndUser = _endUserService.GetById(UserId);
            var isValid = _endUserService.VerifyRefreshToken(accountEndUser!, receieToken, UserType);
            if (isValid)
            {
                _endUserService.GenerateToken(accountEndUser!, UserType);
                return Ok(new ResponseMessage(true, "Refresh token successfully.", accountEndUser!.AccountToken));
            }
            return Ok(new ResponseMessage(false, "Your token is invalid."));

        }

        // PUT /<EndUserController>/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "EndUser")]
        public IActionResult ChangePassword(ChangePasswordRecord changePasswordRecord)
        {
            var validateSummary = _endUserService.ValidateChangePasswordRecord(changePasswordRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var accountEndUser = _endUserService.GetById(UserId);
            var isMatched = _endUserService.VerifyCurrentPassword(accountEndUser!, changePasswordRecord);
            if (isMatched)
            {
                _endUserService.ChangePassword(accountEndUser!, changePasswordRecord);
                return Ok(new ResponseMessage(true, "Change password successfully.", accountEndUser));
            }
            return BadRequest(new ResponseMessage(false, "Your current password is incorrect."));
        }

        // POST /<EndUserController>/ResetPassword
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordRecord resetPasswordRecord)
        {
            var validateSummary = _endUserService.ValidateResetPasswordRecord(resetPasswordRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));

            }
            var accountEndUser = _endUserService.GetByUserName(resetPasswordRecord.UserName!);
            if (accountEndUser != null && accountEndUser.IsVerified)
            {
                _endUserService.SetNewPasswordPending(accountEndUser, resetPasswordRecord);
                _otpService.GenerateResetPasswordOtp(accountEndUser);
                _otpService.SendResetPasswordOTP(accountEndUser);
                // _emailService.SendResetPasswordOTP(accountEndUser);
                return Ok(new ResponseMessage { Success = true, Message = "Please check your email to verify this change." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "UserName does not exist" });
        }

        // POST /<EndUserController>/VerifyResetPassword
        [HttpPost("VerifyResetPassword")]
        public IActionResult VerifyResetPassword([FromBody] VerifyResetPasswordRecord verifyResetRecord)
        {
            var validateSummary = _endUserService.ValidateVerifyResetPasswordRecord(verifyResetRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));

            }
            var accountEndUser = _endUserService.GetByUserName(verifyResetRecord.UserName!);
            var isMatched = _endUserService.VerifyResetPassword(accountEndUser!, verifyResetRecord);
            if (isMatched)
            {
                _endUserService.ApproveNewPassword(accountEndUser!);
                return Ok(new ResponseMessage(true, "Change password successfully.", new { Account = accountEndUser, Token = accountEndUser!.AccountToken }));
            }
            return BadRequest(new ResponseMessage(false, "Incorrect OTP."));
        }

        // PUT /<EndUserController>/Update
        [HttpPut("Update")]
        [Authorize(AuthenticationSchemes = "EndUser")]
        public IActionResult Update([FromBody] AccountUpdateRecord accountUpdateRecord)
        {
            var validateResult = _endUserService.ValidateAccountUpdateRecord(accountUpdateRecord);
            if (!validateResult.IsValid)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = validateResult.ErrorMessage });
            }
            AccountEndUser? accountEndUser = _endUserService.GetById(UserId);
            if (accountEndUser != null)
            {
                accountEndUser = _endUserService.UpdateAccount(accountEndUser, accountUpdateRecord);
                return Ok(new ResponseMessage
                {
                    Success = true,
                    Message = "Update successfully.",
                    Data = new { Account = accountEndUser }
                });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid UserId." });
        }
        #endregion

        #region Store
        // GET /<EndUserController>/Store/All
        [HttpGet("Store/All")]
        [Authorize(AuthenticationSchemes = "Account")]
        public async Task<IActionResult> GetAllStore()
        {
            var cacheId = $"EndUser_StoreList";
            var stores = await _cache.GetRecordAsync<List<StoreReturn_E>>(cacheId);
            if (stores == null)
            {
                stores = _storeService.E_GetAll();
                await _cache.SetRecordAsync(cacheId, stores, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(1)).ConfigureAwait(false);
            }
            return Ok(new ResponseMessage { Success = true, Message = "Get the store list successfully.", Data = new { Stores = stores } });
        }
        #endregion

        #region Game
        // GET /<EndUserController>/Game/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpGet("Game/{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "EndUser")]
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
