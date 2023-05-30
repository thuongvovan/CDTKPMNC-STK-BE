using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.BusinessServices.AccountServices
{
    public class AccountService<TAccount> : CommonService where TAccount : Account
    {
        private readonly IAccountRepository<Account> _accountRepo;
        private readonly JwtAuthen _jwtAuthen;

        public AccountService(IUnitOfWork unitOfWork, JwtAuthen jwtAuthen) : base(unitOfWork)
        {
            _accountRepo = _unitOfWork.AccountRepo;
            _jwtAuthen = jwtAuthen;

        }

        public ValidationSummary ValidateLoginRecord(LoginRecord? loginRecord)
        {
            if (loginRecord == null)
            {
                return new ValidationSummary(false, "Login infomation is required.");
            }
            var validator = new LoginRecordValidator();
            var result = validator.Validate(loginRecord);
            return result.GetSummary();
        }

        public bool VerifyLogin(TAccount? account, LoginRecord loginRecord)
        {
            if (account != null && account.Password == loginRecord.Password!.ToHashSHA256() && account.IsVerified)
            {
                return true;
            }
            return false;
        }

        public void Logout(TAccount account)
        {
            if (account != null)
            {
                account.Otp = null;
                _accountRepo.Update(account);
            }
        }

        // Không sử dụng vì varify token nếu token trống hoặc không hợp lệ đều chung 1 kết quả là faild rồi
        public ValidationSummary ValidateTokenRecord(TokenRecord? tokenRecord)
        {
            if (tokenRecord == null)
            {
                return new ValidationSummary(false, "Token is required.");
            }
            var validator = new TokenRecordValidator();
            var result = validator.Validate(tokenRecord);
            return result.GetSummary();
        }

        public bool VerifyRefreshToken(TAccount account, AccountToken receieToken, AccountType userType)
        {
            if (account != null && account.AccountToken!.AccessToken == receieToken.AccessToken &&
                                        account.AccountToken.RefreshToken == receieToken.RefreshToken)
            {
                var userId = _jwtAuthen.VerifyRefreshToken(receieToken.RefreshToken!, userType);
                if (userId != null) return true;
            }
            return false;
        }

        public void GenerateToken(TAccount account, AccountType userType)
        {
            AccountToken accountToken = _jwtAuthen.GenerateUserToken(account.Id, userType);
            if (account.AccountToken == null)
            {
                account.AccountToken = accountToken;
            }
            else
            {
                account!.AccountToken.AccessToken = accountToken.AccessToken;
                account!.AccountToken.RefreshToken = accountToken.RefreshToken;
            }
            _accountRepo.Update(account);
        }

        public ValidationSummary ValidateChangePasswordRecord(ChangePasswordRecord? changePasswordRecord)
        {
            if (changePasswordRecord == null)
            {
                return new ValidationSummary(false, "Current password and new password are required.");
            }
            var validator = new ChangePasswordRecordValidator();
            var result = validator.Validate(changePasswordRecord);
            return result.GetSummary();
        }

        public bool VerifyCurrentPassword(TAccount account, ChangePasswordRecord changePasswordRecord)
        {
            if (account != null && account.IsVerified && account.Password == changePasswordRecord!.OldPassword!.ToHashSHA256())
            {
                return true;
            }
            return false;
        }

        public void ChangePassword(TAccount account, ChangePasswordRecord changePasswordRecord)
        {
            account.Password = changePasswordRecord!.NewPassword!.ToHashSHA256();
            _accountRepo.Update(account);
        }

        public void ChangePassword(TAccount account, string newPassword)
        {
            account.Password = newPassword.ToHashSHA256();
            _accountRepo.Update(account);
        }

        public void SetNewPasswordPending(TAccount account, ResetPasswordRecord resetPasswordRecord)
        {
            account.NewPassword = resetPasswordRecord.NewPassword!.ToHashSHA256();
            _accountRepo.Update(account);
        }

        public void ApproveNewPassword(TAccount account)
        {
            account.Password = account.NewPassword;
            account.NewPassword = null;
            _accountRepo.Update(account);
        }

        public ValidationSummary ValidateResetPasswordRecord(ResetPasswordRecord? resetPasswordRecord)
        {
            if (resetPasswordRecord == null)
            {
                return new ValidationSummary(false, "User name and new password are required.");
            }
            var validator = new ResetPasswordRecordValidator();
            var result = validator.Validate(resetPasswordRecord);
            return result.GetSummary();
        }

        public ValidationSummary ValidateVerifyResetPasswordRecord(VerifyResetPasswordRecord? verifyReset)
        {
            if (verifyReset == null)
            {
                return new ValidationSummary(false, "User name and new password are required.");
            }
            var validator = new VerifyResetPasswordRecordValidator();
            var result = validator.Validate(verifyReset);
            return result.GetSummary();
        }

        public bool VerifyResetPassword(TAccount account, VerifyResetPasswordRecord verifyReset)
        {
            if (account != null && account.IsVerified && account.NewPassword != null && account.Otp != null && account.Otp.ResetPasswordOtp == verifyReset.Otp && account.Otp.ResetPasswordExpiresOn >= DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public Account? GetById(Guid accountId)
        {
            return _accountRepo.GetById(accountId);
        }

    }
}
