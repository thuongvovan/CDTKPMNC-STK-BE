
using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.Utilities.Email;
using CDTKPMNC_STK_BE.Utilities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities.AccountUtils;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _mailler;
        private readonly JwtAuthen _jwtAuthen;
        public PartnerController(IUnitOfWork unitOfWork, IEmailService mailler, JwtAuthen jwtAuthen)
        {
            _unitOfWork = unitOfWork;
            _mailler = mailler;
            _jwtAuthen = jwtAuthen;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] PartnerRegistrationInfo account)
        {
            if (account == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Missing information." });
            }
            if (account.UserName?.StartsWith("testpartner@") ?? false)
            {
                AccountPartner? currAccountTest = _unitOfWork.AccountPartnerRepository.GetByUserName(account.UserName);
                if (currAccountTest != null)
                {
                    if (currAccountTest?.IsVerified == true)
                        return Conflict(new ResponseMessage { Success = false, Message = "UserName already exists." });
                    _unitOfWork.AccountPartnerRepository.Delete(currAccountTest!);
                }

                var testAccount = new AccountPartner
                {
                    UserName = account.UserName,
                    Name = "Tài Khoản Test Partner",
                    Password = "123456".ToHashSHA256(),
                    Otp = new OtpAccount { RegisterOtp = 123456, RegisterExpiresOn = DateTime.Now.AddHours(2) },
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                    Gender = Gender.Others,
                    CreatedAt = DateTime.Now
                };
                _unitOfWork.AccountPartnerRepository.Add(testAccount);
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
                var validator = new AccountPartnerValidation();
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

                AccountPartner? currAccount = _unitOfWork.AccountPartnerRepository.GetByUserName(account.UserName!);
                if (currAccount != null)
                {
                    if (currAccount?.IsVerified == true)
                        return Conflict(new ResponseMessage { Success = false, Message = "UserName already exists." });
                    else
                        _unitOfWork.AccountPartnerRepository.Delete(currAccount!);
                }

                AddressWard? ward = _unitOfWork.AddressRepository.GetWardById(account.Address.WardId);
                if (ward != null)
                {
                    AccountPartner? newAccount = account.CreateUserAccount(ward);
                    if (newAccount.PertnerType == PartnerType.Personal)
                    {
                        int otp = OTPHelper.GenerateOtp();
                        newAccount.Otp = new OtpAccount { RegisterOtp = otp, RegisterExpiresOn = DateTime.Now.AddMinutes(10) };
                        _unitOfWork.AccountPartnerRepository.Add(newAccount);
                        _mailler.SendRegisterOTP(newAccount);
                        _unitOfWork.Commit();
                        return Ok(new ResponseMessage
                        {
                            Success = true,
                            Message = "Next step is verify user account in 10 minutes",
                            Data = new { UserAccount = newAccount }
                        });
                    }
                    return Ok(new ResponseMessage
                    {
                        Success = true,
                        Message = "Next step, update your company information.",
                        Data = new { UserAccount = newAccount }
                    });
                }
                return BadRequest(new ResponseMessage
                {
                    Success = false,
                    Message = "Invalid information"
                });
            }
        }

        // POST /<UserController>/RegisterCompany/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("RegisterCompany/{userId:Guid}")]
        public IActionResult RegisterCompany(Guid userId, [FromBody] CompanyRegistrationInfo companyInfo)
        {
            var validator = new CompanyValidation();
            ValidationResult? validateResult;
            try
            {
                validateResult = validator.Validate(companyInfo);
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

            Company? currCompany = _unitOfWork.CompanyRepository.GetCompanyByNameOrBusinessCode(companyInfo.Name, companyInfo.BusinessCode);
            AddressWard? companyWard = _unitOfWork.AddressRepository.GetWardById(companyInfo.Address.WardId);
            AccountPartner? currAccount = _unitOfWork.AccountPartnerRepository.GetById(userId);
            if (currCompany != null && companyWard != null && currAccount != null && currAccount.Company == null)
            {
                Company company = companyInfo.CreateCompany(companyWard);
                currAccount.Company = company;
                int otp = OTPHelper.GenerateOtp();
                currAccount.Otp = new OtpAccount { RegisterOtp = otp, RegisterExpiresOn = DateTime.Now.AddMinutes(10) };
                _unitOfWork.AccountPartnerRepository.Add(currAccount);
                _mailler.SendRegisterOTP(currAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage
                {
                    Success = true,
                    Message = "Next step is verify user account in 10 minutes",
                    Data = new { UserAccount = currAccount }
                });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid information." });
        }

        // POST /<UserController>/VerifyRegister/AB8D4730-8895-4C18-F0F8-08DB439AD21D
        [HttpPost("VerifyRegister/{userId:Guid}")]
        public IActionResult VerifyRegister(Guid userId, [FromBody] Otp otp)
        {
            AccountPartner? userAccount = _unitOfWork.AccountPartnerRepository.GetById(userId);
            if (userAccount != null && userAccount.IsVerified == false)
            {
                if (userAccount.Otp != null && userAccount.Otp.RegisterOtp == otp.OtpValue && userAccount.Otp.RegisterExpiresOn >= DateTime.Now)
                {
                    userAccount.IsVerified = true;
                    userAccount.VerifiedAt = DateTime.Now;
                    TokenAccount userToken = _jwtAuthen.GenerateUserToken(userAccount.Id, UserType.Partner);
                    userAccount.AccountToken = userToken;
                    _unitOfWork.AccountPartnerRepository.Update(userAccount);
                    _unitOfWork.Commit();
                    return Ok(new ResponseMessage
                    {
                        Success = true,
                        Message = "Verify successfull.",
                        Data = userAccount
                    });
                }
                else return BadRequest(new ResponseMessage { Success = false, Message = "Not fully registered or invalid OTP." });
            }
            return Conflict(new ResponseMessage { Success = false, Message = "Verification failed." });
        }

        // POST /<UserController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginedAccount account)
        {
            if (account == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "Your information is missing." });
            }
            AccountPartner? currAccount = _unitOfWork.AccountPartnerRepository.GetByUserName(account.UserName);
            if (currAccount != null && currAccount.Password == account.Password.ToHashSHA256() && currAccount.IsVerified)
            {
                TokenAccount userToken = _jwtAuthen.GenerateUserToken(currAccount.Id, UserType.Partner);
                currAccount.AccountToken!.AccessToken = userToken.AccessToken;
                currAccount.AccountToken.RefreshToken = userToken.RefreshToken;
                _unitOfWork.AccountPartnerRepository.Update(currAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Login successfull.", Data = currAccount });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "UserName or password is incorrect." });
        }

        // POST /<UserController>/RefreshToken
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes = "PartnerNoLifetime")]
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
            AccountPartner? account = _unitOfWork.AccountPartnerRepository.GetById(userId!.Value);
            if (account != null && account.AccountToken!.AccessToken == currentToken.AccessToken && account.AccountToken.RefreshToken == currentToken.RefreshToken)
            {
                TokenAccount userToken = _jwtAuthen.GenerateUserToken(account.Id, UserType.Partner);
                account!.AccountToken!.AccessToken = userToken.AccessToken;
                account.AccountToken.RefreshToken = userToken.RefreshToken;
                _unitOfWork.AccountPartnerRepository.Update(account);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Token refresh successful", Data = userToken });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid Refresh AccountToken." });

        }

        // PUT /<UserController>/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Partner")]
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
            AccountPartner? currAccount = _unitOfWork.AccountPartnerRepository.GetById(userId!.Value);
            if (currAccount != null && currAccount.Password == changePasswordAccount.OldPassword.ToHashSHA256() && currAccount.IsVerified)
            {
                currAccount.Password = changePasswordAccount.NewPassword.ToHashSHA256();
                _unitOfWork.AccountPartnerRepository.Update(currAccount);
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
            AccountPartner? currAccount = _unitOfWork.AccountPartnerRepository.GetByUserName(resetPasswordAccount.UserName);
            if (currAccount != null && currAccount.IsVerified)
            {
                currAccount.NewPassword = resetPasswordAccount.NewPassword.ToHashSHA256();
                int resetPasswordOTP = OTPHelper.GenerateOtp();
                currAccount.Otp!.ResetPasswordOtp = resetPasswordOTP;
                currAccount.Otp.ResetPasswordExpiresOn = DateTime.Now.AddMinutes(10);
                _unitOfWork.AccountPartnerRepository.Update(currAccount);
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
            AccountPartner? userAccount = _unitOfWork.AccountPartnerRepository.GetByUserName(verifyReset.UserName);
            if (userAccount != null && userAccount.IsVerified && userAccount.NewPassword != null &&
                userAccount.Otp != null && userAccount.Otp.ResetPasswordOtp == verifyReset.Otp && userAccount.Otp.ResetPasswordExpiresOn > DateTime.Now)
            {
                userAccount.Password = userAccount.NewPassword;
                userAccount.NewPassword = null;
                _unitOfWork.AccountPartnerRepository.Update(userAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Verify successfull. Your password has been changed." });
            }    
            return BadRequest(new ResponseMessage { Success = false, Message = "Verify reset password failed." });
        }

        // POST /<UserController>/Store/Register
        [HttpPost("Store/Register")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult RegisterStore([FromBody] StoreRegistrationInfo storeInfo)
        {
            var validator = new StoreValidation();
            ValidationResult? validateResult;
            try
            {
                validateResult = validator.Validate(storeInfo);
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

            var userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            AccountPartner? account = _unitOfWork.AccountPartnerRepository.GetById(userId!.Value);
            AddressWard? storeWard = _unitOfWork.AddressRepository.GetWardById(storeInfo.Address.WardId);
            Store? currStore = _unitOfWork.StoreRepository.GetStoreByName(storeInfo.Name);
            if (currStore == null && storeWard != null && account != null)
            {
                Store newStore = storeInfo.CreateStore(storeWard);
                account.Store = newStore;
                _unitOfWork.AccountPartnerRepository.Update(account);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage
                {
                    Success = true,
                    Message = "Store created successfully. The Store will operate if approved by admin.",
                    Data = new
                    {
                        NewStore = account.Store
                    }
                });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid information." });
        }

        // PUT /<UserController>/Store/Enable/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Enable/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult ApproveStore(Guid storeId)
        {
            var store = _unitOfWork.StoreRepository.GetStoreById(storeId);
            if (store == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "storeId is not valid." });
            }
            var userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            AccountPartner? account = _unitOfWork.AccountPartnerRepository.GetById(userId!.Value);
            if (store.AccountPartner.Id == account!.Id)
            {
                _unitOfWork.StoreRepository.EnableStore(store);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Enabled store successfully.", Data = new { Store = store } });

            }
            return BadRequest(new ResponseMessage { Success = false, Message = "storeId is not valid." });
        }

        // PUT /<UserController>/Store/Disbale/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Disbale/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Partner")]
        public IActionResult RejectStore(Guid storeId)
        {
            var store = _unitOfWork.StoreRepository.GetStoreById(storeId);
            if (store == null)
            {
                return BadRequest(new ResponseMessage { Success = false, Message = "storeId is not valid." });
            }
            var userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            AccountPartner? account = _unitOfWork.AccountPartnerRepository.GetById(userId!.Value);
            if (store.AccountPartner.Id == account!.Id)
            {
                _unitOfWork.StoreRepository.DisableStore(store);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Enabled store successfully.", Data = new { Store = store } });

            }
            return BadRequest(new ResponseMessage { Success = false, Message = "storeId is not valid." });
        }
    }

}
