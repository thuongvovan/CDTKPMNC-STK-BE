using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.Utilities;


namespace CDTKPMNC_STK_BE.BusinessServices.AccountServices
{
    public class AdminService : AccountService<AccountAdmin>
    {
        private readonly IAccountAdminRepository _accountAdminRepo;
        private readonly AddressService _addressService;
        public AdminService(IUnitOfWork unitOfWork, AddressService addressService, JwtAuthen jwtAuthen) : base(unitOfWork, jwtAuthen)
        {
            _accountAdminRepo = _unitOfWork.AccountAdminRepo;
            _addressService = addressService;
        }

        public List<AccountAdmin> GetAll()
        {
            return _accountAdminRepo.GetAll().ToList();
        }

        new public AccountAdmin? GetById(Guid id)
        {
            return _accountAdminRepo.GetById(id);
        }

        public AccountAdmin? GetByUserName(string userName) 
        {
            return _accountAdminRepo.GetByUserName(userName);
        }

        public ValidationSummary ValidateAdminUpdateRecord(AdminUpdateRecord adminUpdateRecord)
        {
            if (adminUpdateRecord == null)
            {
                return new ValidationSummary(false, "Login infomation is required.");
            }
            var validator = new AdminUpdateRecordValidator(_addressService);
            var result = validator.Validate(adminUpdateRecord);
            return result.GetSummary();
        }

        public AccountAdmin UpdateAccount(AccountAdmin accountAdmin, AdminUpdateRecord adminUpdateRecord)
        {
            accountAdmin.Name = adminUpdateRecord.AccountUpdate!.Name;
            accountAdmin.DateOfBirth = adminUpdateRecord.AccountUpdate.DateOfBirth!.ToDateOnly();
            accountAdmin.Gender = adminUpdateRecord.AccountUpdate!.Gender!.Value;
            accountAdmin.Address.WardId = adminUpdateRecord.AccountUpdate!.Address!.WardId;
            accountAdmin.Address.Street = adminUpdateRecord.AccountUpdate!.Address!.Street;
            accountAdmin.Position = adminUpdateRecord.Position!;
            accountAdmin.Department = adminUpdateRecord.Department!;
            _accountAdminRepo.Update(accountAdmin);
            return accountAdmin;
        }

        //public void GenerateToken(AccountAdmin accountAdmin)
        //{
        //    AccountToken accountToken = _jwtAuthen.GenerateUserToken(accountAdmin.Id, AccountType.Admin);
        //    accountAdmin.AccountToken = accountToken;
        //    _accountAdminRepo.Update(accountAdmin);
        //}

        //public ValidationSummary ValidateChangePasswordRecord(ChangePasswordRecord? changePasswordRecord)
        //{
        //    if (changePasswordRecord == null)
        //    {
        //        return new ValidationSummary(false, "Current password and new password are required.");
        //    }
        //    var validator = new ChangePasswordValidator();
        //    var result = validator.Validate(changePasswordRecord);
        //    return result.GetSummary();
        //}

        //public bool VerifyCurrentPassword(AccountAdmin accountAdmin, ChangePasswordRecord changePasswordRecord)
        //{
        //    if (accountAdmin != null && accountAdmin.IsVerified && accountAdmin.Password == changePasswordRecord.OldPassword.ToHashSHA256())
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public void ChangePassword(AccountAdmin accountAdmin, ChangePasswordRecord changePasswordRecord)
        //{
        //    accountAdmin.Password = changePasswordRecord.NewPassword.ToHashSHA256();
        //    _unitOfWork.Commit(); 
        //}

        //public void ChangePassword(AccountAdmin accountAdmin, string newPassword)
        //{
        //    accountAdmin.Password = newPassword.ToHashSHA256();
        //    _unitOfWork.Commit();
        //}

        //public void ChangePassword(AccountAdmin accountAdmin)
        //{
        //    accountAdmin.Password = accountAdmin.NewPassword;
        //    accountAdmin.NewPassword = null;
        //    _unitOfWork.Commit();
        //}
    }
}
