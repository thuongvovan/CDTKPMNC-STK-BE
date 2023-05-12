using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CDTKPMNC_STK_BE.Models
{
    public class AppBaseController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;
        public Guid UserId { get; set; }
        public AccountType UserType { get; set; }
        public AppBaseController(IUnitOfWork unitOfWork) // IEmailService mailler
        {
            _unitOfWork = unitOfWork;
            UserId =  HttpContext.Items["UserId"]!.ToString()!.ToGuid()!.Value;
            var userType = HttpContext.Items["AccountType"]!.ToString();
            if (userType != null)
            {
                UserType = Enum.Parse<AccountType>(userType);
            }
        }
    }
}
