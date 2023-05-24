using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class OtpService : CommonService
    {
        public OtpService(IUnitOfWork unitOfWork) : base(unitOfWork){}

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
