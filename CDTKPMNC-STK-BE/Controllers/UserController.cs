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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _mailler;
        private readonly JwtAuthen _jwtAuthen;
        public UserController(IUnitOfWork unitOfWork, IEmailService mailler, JwtAuthen jwtAuthen)
        {
            _unitOfWork = unitOfWork;
            _mailler = mailler;
            _jwtAuthen = jwtAuthen;
        }

        // POST /<UserController>/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisteredAccount account)
        {
            if (account.Account?.StartsWith("test@") ?? false)
            {
                EndUserAccount? currAccountTest = _unitOfWork.EndUserAccountRepository.GetByAccount(account.Account);
                if (currAccountTest != null)
                {
                    if (currAccountTest?.IsVerified == true)
                        return Conflict(new ResponseMessage { Success = false, Message = "Account already exists." });
                    _unitOfWork.EndUserAccountRepository.Delete(currAccountTest!);
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
                _unitOfWork.EndUserAccountRepository.Add(testAccount);
                _unitOfWork.Commit();
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

                EndUserAccount? currAccount = _unitOfWork.EndUserAccountRepository.GetByAccount(account.Account!);
                if (currAccount != null)
                {
                    if (currAccount?.IsVerified == true)
                        return Conflict(new ResponseMessage {Success = false, Message = "Account already exists." });
                    else
                        _unitOfWork.EndUserAccountRepository.Delete(currAccount!);
                }

                EndUserAccount? newAccount = account.CreateUserAccount();

                // int otp = new Cryptography(_configuration).GenerateOTP();
                int otp = OTPHelper.GenerateOTP();
                newAccount.OTP = new UserOTP { RegisterOTP = otp, RegisterExpiresOn = DateTime.Now.AddMinutes(10) };
                _unitOfWork.EndUserAccountRepository.Add(newAccount);
                _mailler.SendRegisterOTP(newAccount);
                _unitOfWork.Commit();
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
            EndUserAccount? userAccount = _unitOfWork.EndUserAccountRepository.GetById(userId);
            if (userAccount == null)
                return BadRequest(new ResponseMessage {Success = false, Message = "Account registration is required." });
            if (userAccount != null && userAccount?.IsVerified == false)
            {
                
                if (userAccount?.OTP?.RegisterOTP == otp.OTPValue && userAccount.OTP.RegisterExpiresOn >= DateTime.Now)
                {
                    userAccount.IsVerified = true;
                    UserToken userToken = _jwtAuthen.GenerateUserToken(userAccount.Id, UserType.EndUser);
                    userAccount.Token = userToken;
                    _unitOfWork.EndUserAccountRepository.Update(userAccount);
                    _unitOfWork.Commit();
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
            EndUserAccount? currAccount = _unitOfWork.EndUserAccountRepository.GetByAccount(account.Account);
            if (currAccount != null && currAccount.Password == account.Password.ToHashSHA256() && currAccount.IsVerified)
            {
                UserToken userToken = _jwtAuthen.GenerateUserToken(currAccount.Id, UserType.EndUser);
                currAccount.Token!.AccessToken = userToken.AccessToken;
                currAccount.Token.RefreshToken = userToken.RefreshToken;
                _unitOfWork.EndUserAccountRepository.Update(currAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage {Success = true, Message = "Login successfull.", Data = userToken });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Login information is incorrect." });
        }

        // POST /<UserController>/RefreshToken
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes = "EndUserNoLifetime")]
        public IActionResult Refresh([FromBody] RefreshToken refreshToken)
        {
            var currentUserToken = new UserToken
            {
                RefreshToken = refreshToken.Token,
                AccessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };

            var userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            EndUserAccount? account = _unitOfWork.EndUserAccountRepository.GetById(userId!.Value);
            if (account == null || account.Token!.AccessToken != currentUserToken.AccessToken || account.Token.RefreshToken != currentUserToken.RefreshToken)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Invalid Refresh Token." });
            }

            UserToken userToken = _jwtAuthen.GenerateUserToken(account.Id, UserType.EndUser);
            account!.Token!.AccessToken = userToken.AccessToken;
            account.Token.RefreshToken = userToken.RefreshToken;
            _unitOfWork.EndUserAccountRepository.Update(account);
            _unitOfWork.Commit();
            return Ok(new ResponseMessage { Success = true, Message = "token refresh successful", Data = userToken });
        }

        // PUT /<UserController>/ChangePassword
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordAccount changePasswordAccount)
        {
            var validator = new UserChangePasswordValidation();
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
            EndUserAccount? currAccount = _unitOfWork.EndUserAccountRepository.GetByAccount(changePasswordAccount.Account);
            if (currAccount == null || currAccount.Password != changePasswordAccount.OldPassword.ToHashSHA256() || !currAccount.IsVerified)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Current account or password is incorrect" });
            }
            currAccount.Password = changePasswordAccount.NewPassword.ToHashSHA256();
            _unitOfWork.EndUserAccountRepository.Update(currAccount);
            _unitOfWork.Commit();
            return Ok(new ResponseMessage { Success = true, Message = "Change password successful" });
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

                return BadRequest(new ResponseMessage { Success = false, Message = "Unable to verify data" });
            }

            if (!validateResult.IsValid)
            {
                string? ErrorMessage = validateResult.Errors?.FirstOrDefault()?.ErrorMessage;
                return BadRequest(new ResponseMessage { Success = false, Message = ErrorMessage! });
            }
            EndUserAccount? currAccount = _unitOfWork.EndUserAccountRepository.GetByAccount(resetPasswordAccount.Account);
            if (currAccount == null || !currAccount.IsVerified)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Account does not exist" });
            }
            else
            {
                currAccount.NewPassword = resetPasswordAccount.NewPassword.ToHashSHA256();
                int resetPasswordOTP = OTPHelper.GenerateOTP();
                currAccount.OTP!.ResetPasswordOTP = resetPasswordOTP;
                currAccount.OTP.ResetPasswordExpiresOn = DateTime.Now.AddMinutes(10);
                // _unitOfWork.EndUserAccountRepository.Update(currAccount);
                _unitOfWork.Commit();
                var emailTask = Task.Run(() =>
                {
                    _mailler.SendResetPasswordOTP(currAccount);
                });
                emailTask.Wait();
                return Ok(new ResponseMessage { Success = true, Message = "Please check your email to verify this change." });
            }
        }

        // POST /<UserController>/VerifyRegister/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("VerifyResetPassword")]
        public IActionResult VerifyResetPassword([FromBody] VerifyResetPwAccount verifyReset)
        {
            EndUserAccount? userAccount = _unitOfWork.EndUserAccountRepository.GetByAccount(verifyReset.Account);
            if (userAccount == null || userAccount.IsVerified == false ||
                userAccount.NewPassword == null ||
                userAccount.OTP == null || userAccount.OTP.ResetPasswordOTP == null || userAccount.OTP.ResetPasswordExpiresOn == null)
                return BadRequest(new ResponseMessage { Success = false, Message = "Verify reset password failed." });
            if (userAccount.OTP.ResetPasswordOTP == verifyReset.OTP && userAccount.OTP.ResetPasswordExpiresOn >= DateTime.Now)
            {
                userAccount.Password = userAccount.NewPassword;
                userAccount.NewPassword = null;
                _unitOfWork.EndUserAccountRepository.Update(userAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Verify successfull." });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Verify reset password failed." });
        }
    }
} 
