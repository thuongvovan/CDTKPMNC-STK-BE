using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using CDTKPMNC_STK_BE.Utilities.Email;
using CDTKPMNC_STK_BE.Utilities.AccountUtils;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EndUserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _mailler;
        private readonly JwtAuthen _jwtAuthen;
        public EndUserController(IUnitOfWork unitOfWork, IEmailService mailler, JwtAuthen jwtAuthen)
        {
            _unitOfWork = unitOfWork;
            _mailler = mailler;
            _jwtAuthen = jwtAuthen;
        }

        // POST /<UserController>/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] EndUserRegistrationInfo account)
        {
            if (account == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Do you need to provide posting information." });
            }
            if (account.UserName?.StartsWith("test@") ?? false)
            {
                AccountEndUser? currAccountTest = _unitOfWork.AccountEndUserRepository.GetByUserName(account.UserName);
                if (currAccountTest != null)
                {
                    if (currAccountTest?.IsVerified == true)
                        return Conflict(new ResponseMessage { Success = false, Message = "UserName already exists." });
                    _unitOfWork.AccountEndUserRepository.Delete(currAccountTest!);
                }

                var testAccount = new AccountEndUser 
                    {
                        UserName = account.UserName,
                        Name = "Tài Khoản Test",
                        Password = "123456".ToHashSHA256(),
                        Otp = new OtpAccount { RegisterOtp = 123456, RegisterExpiresOn = DateTime.Now.AddHours(2) },
                        DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                        Gender = Gender.Others,
                        CreatedAt = DateTime.Now,
                };
                _unitOfWork.AccountEndUserRepository.Add(testAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage 
                    { 
                        Success = true, 
                        Message = "Test account RegisterOtp & Password is: 123456", 
                        Data = new { UserAccount = testAccount }
                    });
            }
            else
            {
                var validator = new AccountUserValidation();
                ValidationResult? validateResult;
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

                AccountEndUser? currAccount = _unitOfWork.AccountEndUserRepository.GetByUserName(account.UserName!);
                if (currAccount != null)
                {
                    if (currAccount?.IsVerified == true)
                        return Conflict(new ResponseMessage {Success = false, Message = "UserName already exists." });
                    else
                        _unitOfWork.AccountEndUserRepository.Delete(currAccount!);
                }

                AddressWard? ward = _unitOfWork.AddressRepository.GetWardById(account.Address.WardId);
                if(ward != null)
                {
                    AccountEndUser newAccount = account.CreateUserAccount(ward);
                    int otp = OTPHelper.GenerateOtp();
                    newAccount.Otp = new OtpAccount { RegisterOtp = otp, RegisterExpiresOn = DateTime.Now.AddMinutes(10) };
                    _unitOfWork.AccountEndUserRepository.Add(newAccount);
                    _mailler.SendRegisterOTP(newAccount);
                    _unitOfWork.Commit();
                    return Ok(new ResponseMessage
                    {
                        Success = true,
                        Message = "Next step is verify user account in 10 minutes",
                        Data = new { UserAccount = newAccount }
                    });
                }
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid information."});
            }
        }


        // POST /<UserController>/VerifyRegister/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("VerifyRegister/{userId:Guid}")]
        public IActionResult VerifyRegister(Guid userId, [FromBody] Otp otp)
        {
            if (otp == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Your information is missing." });
            }
            AccountEndUser? userAccount = _unitOfWork.AccountEndUserRepository.GetById(userId);
            if (userAccount == null)
                return BadRequest(new ResponseMessage {Success = false, Message = "UserId is required." });
            if (userAccount != null && userAccount?.IsVerified == false)
            {
                
                if (userAccount?.Otp?.RegisterOtp == otp.OtpValue && userAccount.Otp.RegisterExpiresOn >= DateTime.Now)
                {
                    userAccount.IsVerified = true;
                    userAccount.VerifiedAt = DateTime.Now;
                    TokenAccount userToken = _jwtAuthen.GenerateUserToken(userAccount.Id, UserType.EndUser);
                    userAccount.AccountToken = userToken;
                    _unitOfWork.AccountEndUserRepository.Update(userAccount);
                    _unitOfWork.Commit();
                    return Ok(new ResponseMessage 
                        { 
                            Success = true,
                            Message = "Verify successfull.", 
                            Data = userToken
                        });
                }
                else return BadRequest(new ResponseMessage {Success = false, Message = "Your RegisterOtp is not correct or expiresed" });
            }
            return Conflict(new ResponseMessage {Success = false, Message = "Verification failed." });
        }

        // POST /<UserController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginedAccount account)
        {
            if (account == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Your information is missing." });
            }
            AccountEndUser? currAccount = _unitOfWork.AccountEndUserRepository.GetByUserName(account.UserName);
            if (currAccount != null && currAccount.Password == account.Password.ToHashSHA256() && currAccount.IsVerified)
            {
                TokenAccount userToken = _jwtAuthen.GenerateUserToken(currAccount.Id, UserType.EndUser);
                currAccount.AccountToken!.AccessToken = userToken.AccessToken;
                currAccount.AccountToken.RefreshToken = userToken.RefreshToken;
                _unitOfWork.AccountEndUserRepository.Update(currAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage {Success = true, Message = "Login successfull.", Data = currAccount });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "UserName or password is incorrect." });
        }

        // POST /<UserController>/RefreshToken
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes = "EndUserNoLifetime")]
        public IActionResult Refresh([FromBody] RefreshToken refreshToken)
        {
            if (refreshToken == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Missing information." });
            }
            var currentToken = new TokenAccount
            {
                RefreshToken = refreshToken.Token,
                AccessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };

            var userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            AccountEndUser? account = _unitOfWork.AccountEndUserRepository.GetById(userId!.Value);
            if (account != null && account.AccountToken!.AccessToken == currentToken.AccessToken && account.AccountToken.RefreshToken == currentToken.RefreshToken)
            {
                TokenAccount userToken = _jwtAuthen.GenerateUserToken(account.Id, UserType.EndUser);
                account!.AccountToken!.AccessToken = userToken.AccessToken;
                account.AccountToken.RefreshToken = userToken.RefreshToken;
                _unitOfWork.AccountEndUserRepository.Update(account);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "token refresh successful", Data = userToken });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid Refresh AccountToken." });

        }

        // PUT /<UserController>/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "EndUser")]
        public IActionResult ChangePassword(ChangePasswordAccount changePasswordAccount)
        {
            if (changePasswordAccount == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Missing information." });
            }
            var validator = new ChangePasswordValidation();
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
            AccountEndUser? currAccount = _unitOfWork.AccountEndUserRepository.GetById(userId!.Value);
            if (currAccount != null && currAccount.Password == changePasswordAccount.OldPassword.ToHashSHA256() && !currAccount.IsVerified)
            {
                currAccount.Password = changePasswordAccount.NewPassword.ToHashSHA256();
                _unitOfWork.AccountEndUserRepository.Update(currAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Change password successful" });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Your current password is incorrect." });
        }

        // POST /<UserController>/ResetPassword
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordAccount resetPasswordAccount)
        {
            if (resetPasswordAccount == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Missing information." });
            }
            var validator = new ResetPasswordValidation();
            ValidationResult? validateResult = null;
            try
            {
                validateResult = validator.Validate(resetPasswordAccount);
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
            AccountEndUser? currAccount = _unitOfWork.AccountEndUserRepository.GetByUserName(resetPasswordAccount.UserName);
            if (currAccount != null && currAccount.IsVerified)
            {
                currAccount.NewPassword = resetPasswordAccount.NewPassword.ToHashSHA256();
                int resetPasswordOTP = OTPHelper.GenerateOtp();
                currAccount.Otp!.ResetPasswordOtp = resetPasswordOTP;
                currAccount.Otp.ResetPasswordExpiresOn = DateTime.Now.AddMinutes(10);
                // _unitOfWork.AccountEndUserRepository.Update(currAccount);
                _unitOfWork.Commit();
                var emailTask = Task.Run(() =>
                {
                    _mailler.SendResetPasswordOTP(currAccount);
                });
                emailTask.Wait();
                return Ok(new ResponseMessage { Success = true, Message = "Please check your email to verify this change." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "UserName does not exist" });
        }

        // POST /<UserController>/VerifyResetPassword
        [HttpPost("VerifyResetPassword")]
        public IActionResult VerifyResetPassword([FromBody] VerifyResetPwAccount verifyReset)
        {
            if (verifyReset == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Missing information." });
            }
            AccountEndUser? userAccount = _unitOfWork.AccountEndUserRepository.GetByUserName(verifyReset.UserName);
            if (userAccount != null && userAccount.IsVerified && userAccount.NewPassword != null &&
                userAccount.Otp != null && userAccount.Otp.ResetPasswordOtp == verifyReset.Otp && userAccount.Otp.ResetPasswordExpiresOn > DateTime.Now)
            {
                userAccount.Password = userAccount.NewPassword;
                userAccount.NewPassword = null;
                _unitOfWork.AccountEndUserRepository.Update(userAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Verify successfull. Your password has been changed." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Verify reset password failed." });
        }
    }
} 
