using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class OtpService : CommonService
    {
        private readonly EmailService _emailService;
        public OtpService(IUnitOfWork unitOfWork, EmailService emailService) : base(unitOfWork)
        {
            _emailService = emailService;
        }

        public void GenerateRegisterOtp(Account account)
        {
            if (account.Otp == null)
            {
                account.Otp = new AccountOtp
                {
                    RegisterOtp = RandomHelper.GenerateOtp(),
                    RegisterExpiresOn = DateTime.Now.AddMinutes(10),
                };
            }
            else
            {
                account.Otp.RegisterOtp = RandomHelper.GenerateOtp();
                account.Otp.RegisterExpiresOn = DateTime.Now.AddMinutes(10);
            }
            _unitOfWork.AccountRepo.Update(account);
        }

        public void SendRegisterOTPEndUser(Account userAccount)
        {
            string subject = "[CĐ-TKPMNC] Verify your user account";
            string html = @$"
                Dear {userAccount.Name},<br/>
                
                <br/>
                Thank you for joining us.<br/>
                To verify your email address, enter the verification code below on our application.<br/>
                Your verification code: <b>{userAccount.Otp?.RegisterOtp}</b><br/>
                <br/>

                Thanks you,<br/>
                Thương - Khôi - Sơn
                ";
            if (userAccount.UserName is not null)
            {
                _emailService.Send(userAccount.UserName, subject, html);
            }
        }

        public void SendRegisterOTPPartner(Account userAccount)
        {
            string subject = "[CĐ-TKPMNC] Verify your account";
            string html = @$"
                Dear {userAccount.Name},<br/>
                
                <br/>
                Thanks for joining us!<br/>
                You have registered {userAccount.UserName} as my partner.
                Please enter the verification code below on our application to verify your email address.<br/>
                Your verification code: <b>{userAccount.Otp?.RegisterOtp}</b><br/>
                
                <br/>
                Thanks you,<br/>
                Thương - Khôi - Sơn
                ";
            if (userAccount.UserName is not null)
            {
                _emailService.Send(userAccount.UserName, subject, html);
            }
        }


        public void GenerateResetPasswordOtp(Account account)
        {

            if (account.Otp == null)
            {
                account.Otp = new AccountOtp
                {
                    ResetPasswordOtp = RandomHelper.GenerateOtp(),
                    ResetPasswordExpiresOn = DateTime.Now.AddMinutes(10),
                };
            }
            else
            {
                account.Otp.ResetPasswordOtp = RandomHelper.GenerateOtp();
                account.Otp.ResetPasswordExpiresOn = DateTime.Now.AddMinutes(10);
            }
            _unitOfWork.AccountRepo.Update(account);
        }

        public void SendResetPasswordOTP(Account userAccount)
        {
            string subject = "Your verification for reset password CĐ-TKPMNC";
            string html = @$"
                Dear {userAccount.Name},<br/>
                <br/>
                
                Please enter verifycation code below on our application to reset your password.<br/>
                Your verification code: <b>{userAccount.Otp?.ResetPasswordOtp}</b>.<br/>
                <br/>
                
                Thanks you,<br/>
                Thương - Khôi - Sơn
                ";
            if (userAccount.UserName is not null)
            {
                _emailService.Send(userAccount.UserName, subject, html);
            }
        }


        public bool Verify(Account account, Otp otp)
        {
            if (account.Otp != null && account.Otp.RegisterOtp == otp.OtpValue && account.Otp.RegisterExpiresOn >= DateTime.Now)
            {
                account.IsVerified = true;
                account.VerifiedAt = DateTime.Now;
                _unitOfWork.AccountRepo.Update(account);
                return true;
            }
            return false;
        }
    }
}
