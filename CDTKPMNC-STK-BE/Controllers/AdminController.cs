using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Utilities.Email;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities.Account;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _mailler;
        private readonly JwtAuthen _jwtAuthen;
        private readonly IAccountAdminRepository _adminAccountRepository;
        public AdminController(IUnitOfWork unitOfWork, IEmailService mailler, JwtAuthen jwtAuthen)
        {
            _unitOfWork = unitOfWork;
            _adminAccountRepository = _unitOfWork.AdminAccountRepository;
            _mailler = mailler;
            _jwtAuthen = jwtAuthen;
        }

        // POST /<AdminController>/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginedAccount account)
        {
            var adminAccount = _adminAccountRepository.GetByUserName(account.UserName);
            if (adminAccount != null && account.Password == account.Password.ToHashSHA256() && adminAccount.IsVerified)
            {
                var accountToken = _jwtAuthen.GenerateUserToken(adminAccount.Id, UserType.Admin);
                adminAccount.AccountToken!.AccessToken = accountToken.AccessToken;
                adminAccount.AccountToken.RefreshToken = accountToken.RefreshToken;
                _adminAccountRepository.Update(adminAccount);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Login successfull.", Data = adminAccount });

            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Username or password is incorrect" });
        }

        /*// PUT api/<AdminController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdminController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
