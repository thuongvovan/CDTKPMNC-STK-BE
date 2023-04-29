using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using CDTKPMNC_STK_BE.Utilities.Email;
using CDTKPMNC_STK_BE.Utilities.Account;
using Microsoft.AspNetCore.Authorization;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly EmailService _mailler;
        private readonly Cryptography _cryptography;
        public UserController(AppDBContext dbContext, EmailService mailler, Cryptography cryptography)
        {
            _dbContext = dbContext;
            _mailler = mailler;
            _cryptography = cryptography;
        }

        // POST /<UserController>/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisteredAccount account)
        {
            if (account.Account?.StartsWith("test@") ?? false)
            {
                EndUserAccount? currAccountTest = _dbContext.EndUserAccounts
                                            .SingleOrDefault(ua => ua.Account == account.Account);
                if (currAccountTest != null)
                {
                    if (currAccountTest?.IsVerified == true)
                        return Conflict(new ResponseMessage { Success = false, Message = "Account already exists." });
                    else
                        _dbContext.EndUserAccounts.Remove(currAccountTest!);
                }

                var testAccount = new EndUserAccount 
                    {
                        Account = account.Account,
                        Name = "Tài Khoản Test",
                        Password = "123456".ToHashSHA256(),
                        OTP = new UserOTP { RegisterOTP = 123456, RegisterExpiresOn = DateTime.Now.AddHours(2400) },
                        DateOfBirth = DateTime.Now,
                        Gender = Gender.Others
                    };
                _dbContext.EndUserAccounts.Add(testAccount);
                _dbContext.SaveChangesAsync();
                return Ok(new ResponseMessage 
                    { 
                        Success = true, 
                        Message = "Test account RegisterOTP & Password is: 123456", 
                        Data = new { UserAccount = testAccount }
                    });
            }
            else
            {
                var validator = new UserAccountValidation();
                ValidationResult? validateResult = null;
                try
                {
                    validateResult = validator.Validate(account);
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

                EndUserAccount? currAccount = _dbContext.EndUserAccounts
                                            .SingleOrDefault(ua => ua.Account == account.Account);
                if (currAccount != null)
                {
                    if (currAccount?.IsVerified == true)
                        return Conflict(new ResponseMessage {Success = false, Message = "Account already exists." });
                    else
                        _dbContext.EndUserAccounts.Remove(currAccount!);
                }

                EndUserAccount? newAccount = account.CreateUserAccount();

                // int otp = new Cryptography(_configuration).GenerateOTP();
                int otp = _cryptography.GenerateOTP();
                newAccount.OTP = new UserOTP { RegisterOTP = otp, RegisterExpiresOn = DateTime.Now.AddMinutes(10) };
                _dbContext.EndUserAccounts.Add(newAccount);

                var dbTask = Task.Run(() => { _dbContext.SaveChanges(); });
                var emailTask = Task.Run(() =>
                {
                    _mailler.SendRegisterOTP(newAccount);
                });
                dbTask.Wait();
                emailTask.Wait();
                return Ok(new ResponseMessage 
                    { 
                        Success = true, 
                        Message = "Next step is verify user account in 10 minutes", 
                        Data = new { UserAccount = newAccount }
                    });
            }
        }


        // POST /<UserController>/VerifyRegister/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("VerifyRegister/{userId:Guid}")]
        public IActionResult VerifyRegister(Guid userId, [FromBody] OTP otp)
        {
            EndUserAccount? userAccount = _dbContext.EndUserAccounts
                                        .SingleOrDefault(ua => ua.Id == userId);
            if (userAccount == null)
                return BadRequest(new ResponseMessage {Success = false, Message = "Account registration is required." });
            if (userAccount != null && userAccount?.IsVerified == false)
            {
                if (userAccount?.OTP?.RegisterOTP == otp.OTPValue && userAccount.OTP.RegisterExpiresOn >= DateTime.Now)
                {
                    userAccount.IsVerified = true;
                    UserToken userToken = _cryptography.GenerateUserToken(userAccount.Id, UserType.EndUser);
                    userAccount.Token = userToken;
                    _dbContext.SaveChangesAsync();
                    return Ok(new ResponseMessage 
                        { 
                            Success = true,
                            Message = "Verify successfull.", 
                            Data = userToken
                        });
                }
                else return BadRequest(new ResponseMessage {Success = false, Message = "Your RegisterOTP is not correct or expiresed" });
            }
            return Conflict(new ResponseMessage {Success = false, Message = "Verification failed." });
        }

        // POST /<UserController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginedAccount account)
        {
            EndUserAccount? currAccount = _dbContext
                                        .EndUserAccounts
                                        .SingleOrDefault(ac => ac.Account == account.Account &&
                                                               ac.Password == account.Password.ToHashSHA256() &&
                                                               ac.IsVerified == true);
            if (currAccount != null)
            {
                UserToken userToken = _cryptography.GenerateUserToken(currAccount.Id, UserType.EndUser);
                currAccount.Token = userToken;
                _dbContext.SaveChangesAsync();
                return Ok(new ResponseMessage {Success = true, Message = "Login successfull.", Data = userToken });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Login information is incorrect." });
        }

        // POST /<UserController>/Refresh
        [HttpPost("Refresh")]
        [Authorize(AuthenticationSchemes = "EndUserNoLifetime")]
        public IActionResult Refresh([FromBody] RefreshToken refreshToken)
        {
            var token = refreshToken.Token;
            Guid? userID = _cryptography.ValidateRefreshToken(token ?? "", UserType.EndUser);
            if (userID == null)
            {
                return Unauthorized(new ResponseMessage { Success = false, Message = "Invalid Refresh Token." });
            }
            EndUserAccount? account = _dbContext.EndUserAccounts.SingleOrDefault(u => u.Id == userID);
            if (account == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid Refresh Token." });
            }
            UserToken userToken = _cryptography.GenerateUserToken(account.Id, UserType.EndUser);
            account.Token = userToken;
            _dbContext.SaveChangesAsync();
            return Ok(new ResponseMessage {Success = true,  Message = "token refresh successful", Data = userToken });
        }

        // POST /<UserController>/ChangePassword
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordAccount changePasswordAccount)
        {
            var validator = new UserChangePasswordValidation();
            ValidationResult? validateResult = null;
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
            EndUserAccount? currAccount = _dbContext.EndUserAccounts
                                        .SingleOrDefault(ua => ua.Account == changePasswordAccount.Account &&
                                                               ua.Password == changePasswordAccount.OldPassword.ToHashSHA256() &&
                                                               ua.IsVerified
                                                               );
            if (currAccount == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Current account or password is incorrect" });
            }
            currAccount.Password = changePasswordAccount.OldPassword.ToHashSHA256();
            _dbContext.SaveChangesAsync();
            return Ok(new ResponseMessage { Success =true, Message = "Change password successful" });
        }

        // POST /<UserController>/ChangePassword
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordAccount resetPasswordAccount)
        {
            var validator = new UserResetPasswordValidation();
            ValidationResult? validateResult = null;
            try
            {
                validateResult = validator.Validate(resetPasswordAccount);
            }
            catch (Exception)
            {

                return BadRequest(new ResponseMessage { Success =false, Message = "Unable to verify data" });
            }

            if (!validateResult.IsValid)
            {
                string? ErrorMessage = validateResult.Errors?.FirstOrDefault()?.ErrorMessage;
                return BadRequest(new ResponseMessage { Success =false, Message = ErrorMessage! });
            }
            EndUserAccount? currAccount = _dbContext.EndUserAccounts
                                        .SingleOrDefault(ua => ua.Account == resetPasswordAccount.Account && ua.IsVerified);
            if (currAccount == null)
            {
                return BadRequest(new ResponseMessage {Success = false, Message = "Account does not exist" });
            }
            else
            {
                currAccount.NewPassword = resetPasswordAccount.NewPassword.ToHashSHA256();
                int resetPasswordOTP = _cryptography.GenerateOTP();
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                currAccount.OTP.ResetPasswordOTP = resetPasswordOTP;
                #pragma warning restore CS8602 // Dereference of a possibly null reference.
                currAccount.OTP.ResetPasswordExpiresOn = DateTime.Now.AddMinutes(10);

                var dbTask = Task.Run(() => { _dbContext.SaveChanges(); });
                var emailTask = Task.Run(() =>
                {
                    _mailler.SendResetPasswordOTP(currAccount);
                });
                dbTask.Wait();
                emailTask.Wait();
                return Ok(new ResponseMessage { Success = true, Message = "Please check your email to verify this change." });
            }
        }

        // POST /<UserController>/VerifyRegister/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("VerifyResetPassword")]
        public IActionResult VerifyResetPassword([FromBody] VerifyResetPwAccount verifyReset)
        {
            EndUserAccount? userAccount = _dbContext.EndUserAccounts
                                        .SingleOrDefault(ua => ua.Account == verifyReset.Account &&
                                                         ua.IsVerified &&
                                                         ua.NewPassword != null); 
            if (userAccount == null || userAccount.OTP == null || userAccount.OTP.ResetPasswordOTP == null || userAccount.OTP.ResetPasswordExpiresOn == null)
                return BadRequest(new ResponseMessage { Success = false, Message = "Verify reset password failed." });
            if (userAccount.OTP.ResetPasswordOTP == verifyReset.OTP && userAccount.OTP.ResetPasswordExpiresOn >= DateTime.Now)
            {
                userAccount.Password = userAccount.NewPassword;
                userAccount.NewPassword = null;
                _dbContext.SaveChanges();
                return Ok(new ResponseMessage { Success = true, Message = "Verify successfull." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Verify reset password failed." });
        }
    }
} 
