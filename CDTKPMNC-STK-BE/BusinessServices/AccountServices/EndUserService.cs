using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.AccountServices
{
    public class EndUserService : AccountService<AccountEndUser>
    {
        private readonly IAccountEndUserRepository _accountEndUserRepo;
        private readonly AddressService _addressService;
        public EndUserService(IUnitOfWork unitOfWork, JwtAuthen jwtAuthen, AddressService addressService) : base(unitOfWork, jwtAuthen)
        {
            _accountEndUserRepo = _unitOfWork.AccountEndUserRepo;
            _addressService = addressService;
        }

        public List<AccountEndUser> GetAll()
        {
            var accountEndUsers = _accountEndUserRepo.GetAll().ToList();
            if (accountEndUsers != null)
            {
                return accountEndUsers;
            }
            return new List<AccountEndUser>(0);
        }

        new public AccountEndUser? GetById(Guid id)
        {
            return _accountEndUserRepo.GetById(id);
        }

        public AccountEndUser? GetByUserName(string userName)
        {
            return _accountEndUserRepo.GetByUserName(userName); 
        }

        public void AddAccountEndUser(AccountEndUser accountEndUser)
        {
            _accountEndUserRepo.Add(accountEndUser);
        }

        public void DeleteAccountEndUser(AccountEndUser accountEndUser)
        {
            _accountEndUserRepo.Delete(accountEndUser);
        }

        public void DeleteAccountEndUser(Guid accountEndUserId)
        {
            _accountEndUserRepo.Delete(accountEndUserId);
        }

        public ValidationSummary ValidateUserRegistrationRecord(AccountRegistrationRecord? userRegistrationRecord)
        {
            if (userRegistrationRecord == null)
            {
                return new ValidationSummary(false, "Registration infomation is required.");
            }
            var validator = new AccountRegistrationRecordValidator(_addressService);
            var result = validator.Validate(userRegistrationRecord);
            return result.GetSummary();
        }

        public AccountEndUser CreateAccountEndUser(AccountRegistrationRecord userRegistrationRecord)
        {
            var accountEndUser = new AccountEndUser
            {
                UserName = userRegistrationRecord!.UserName!.ToLower(),
                Password = userRegistrationRecord!.Password!.ToHashSHA256(),
                Name = userRegistrationRecord!.Name!.ToTitleCase(),
                Gender = userRegistrationRecord!.Gender!.Value,
                DateOfBirth = userRegistrationRecord!.DateOfBirth!.ToDateOnly(),
                CreatedAt = DateTime.Now,
                Address = new Address
                {
                    WardId = userRegistrationRecord!.Address!.WardId,
                    Street = userRegistrationRecord!.Address!.Street!.ToTitleCase()
                },
                IsVerified = false,
            };
            _accountEndUserRepo.Add(accountEndUser);
            return accountEndUser;
        }

        public ValidationSummary ValidateAccountUpdateRecord(AccountUpdateRecord? accountUpdateRecord)
        {
            if (accountUpdateRecord == null)
            {
                return new ValidationSummary(false, "Registration infomation is required.");
            }
            var validator = new AccountUpdateRecordValidator(_addressService);
            var result = validator.Validate(accountUpdateRecord);
            return result.GetSummary();
        }

        public AccountEndUser UpdateAccount(AccountEndUser accountEndUser, AccountUpdateRecord accountUpdateRecord)
        {
            accountEndUser.Name = accountUpdateRecord!.Name!.ToTitleCase();
            accountEndUser.Gender = accountUpdateRecord!.Gender!.Value;
            accountEndUser.DateOfBirth = accountUpdateRecord!.DateOfBirth!.ToDateOnly();
            accountEndUser.Address.WardId = accountUpdateRecord!.Address!.WardId;
            accountEndUser.Address.Street = accountUpdateRecord!.Address!.Street!.ToTitleCase();
            _accountEndUserRepo.Update(accountEndUser);
            return accountEndUser;
        }

        #region For Dashboard

        public int CountAll()
        {
            return _accountEndUserRepo.GetAll().Count();
        }

        public int CountVerified()
        {
            return _accountEndUserRepo.GetAll().Where(e => e.IsVerified).Count();
        }

        #endregion

    }
}
