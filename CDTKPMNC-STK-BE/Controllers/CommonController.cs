using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CDTKPMNC_STK_BE.Controllers
{
    public class CommonController : ControllerBase
    {
        public Guid UserId => HttpContext.Items["UserId"]!.ToString()!.ToGuid()!.Value;
        public AccountType UserType 
        { 
            get 
            {
                var userType = HttpContext.Items["AccountType"]!.ToString()!;
                return Enum.Parse<AccountType>(userType);
            } 
        }
        public CommonController() {}
    }
}
