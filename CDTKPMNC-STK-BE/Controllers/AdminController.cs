﻿using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.Utilities.Email;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities.AccountUtils;
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
            _adminAccountRepository = _unitOfWork.AccountAdminRepository;
            // _mailler = mailler;
            _jwtAuthen = jwtAuthen;
        }

        // POST /<AdminController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginedAccount account)
        {
            var adminAccount = _adminAccountRepository.GetByUserName(account.UserName);
            if (adminAccount != null && adminAccount.Password == account.Password.ToHashSHA256() && adminAccount.IsVerified)
            {
                var accountToken = _jwtAuthen.GenerateUserToken(adminAccount.Id, UserType.Admin);
                var userToken = new TokenAccount
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
            var currentToken = new TokenAccount
            {
                RefreshToken = refreshToken.Token,
                AccessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
            };

            var userId = HttpContext.Items["UserId"]!.ToString()!.ToGuid();
            AccountAdmin? account = _unitOfWork.AccountAdminRepository.GetById(userId!.Value);
            if (account != null && account.AccountToken!.AccessToken == currentToken.AccessToken && account.AccountToken.RefreshToken == currentToken.RefreshToken)
            {
                TokenAccount userToken = _jwtAuthen.GenerateUserToken(account.Id, UserType.Admin);
                account!.AccountToken!.AccessToken = userToken.AccessToken;
                account.AccountToken.RefreshToken = userToken.RefreshToken;
                _unitOfWork.AccountAdminRepository.Update(account);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "token refresh successful", Data = userToken });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid Refresh AccountToken." });
        }

        // PUT /<UserController>/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult ChangePassword(ChangePasswordAccount changePasswordAccount)
        {
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
            AccountAdmin? currAccount = _unitOfWork.AccountAdminRepository.GetById(userId!.Value);
            if (currAccount != null && currAccount.Password == changePasswordAccount.OldPassword.ToHashSHA256() && !currAccount.IsVerified)
            {
                currAccount.Password = changePasswordAccount.NewPassword.ToHashSHA256();
                _unitOfWork.AccountAdminRepository.Update(currAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Change password successful" });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Your current password is incorrect." });
        }

        // GET /<UserController>/Partners
        [HttpGet("Partners")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetPartners()
        {
            var partners = _unitOfWork.AccountPartnerRepository.GetAll();
            return Ok(new ResponseMessage { Success = true, Message = "Get the list of all partners successfully.", Data = new { Partners = partners } }); 
        }

        // GET /<UserController>/Store/All
        [HttpGet("Store/All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllStores()
        {
            var stores = _unitOfWork.StoreRepository.GetAll();
            return Ok(new ResponseMessage { Success = true, Message = "Get the list of all stores successfully.", Data = new { Stores = stores } });
        }

        // GET /<UserController>/Store/Approved
        [HttpGet("Store/Approved")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetApprovedStores()
        {
            var stores = _unitOfWork.StoreRepository.GetApproved();
            return Ok(new ResponseMessage { Success = true, Message = "Get the list of approved stores successfully.", Data = new { Stores = stores } });
        }

        // GET /<UserController>/Store/Rejected
        [HttpGet("Store/Rejected")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetRejectedStores()
        {
            var stores = _unitOfWork.StoreRepository.GetRejected();
            return Ok(new ResponseMessage { Success = true, Message = "Get the list of rejected stores successfully.", Data = new { Stores = stores } });
        }

        // GET /<UserController>/Store/NeedApproval
        [HttpGet("Store/NeedApproval")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetNeedApprovalStores()
        {
            var stores = _unitOfWork.StoreRepository.GetNeedApproval();
            return Ok(new ResponseMessage { Success = true, Message = "Get the list of need approval stores successfully.", Data = new { Stores = stores } });
        }

        // PUT /<UserController>/Store/Approve/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Store/Approve/{storeId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult ApproveStore(Guid storeId)
        {
            var store = _unitOfWork.StoreRepository.GetStoreById(storeId);
            if (store != null)
            {
                _unitOfWork.StoreRepository.ApproveStore(store);
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
            var store = _unitOfWork.StoreRepository.GetStoreById(storeId);
            if (store != null)
            {
                _unitOfWork.StoreRepository.RejectStore(store);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Reject store successfully.", Data = new { Store = store } });

            }
            return BadRequest(new ResponseMessage { Success = false, Message = "storeId is not valid." });
        }
    }
}
