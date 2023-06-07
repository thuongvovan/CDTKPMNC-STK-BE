using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Utilities;
using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using FluentValidation;

namespace CDTKPMNC_STK_BE.BusinessServices.AccountServices
{
    public class PartnerService : AccountService<AccountPartner>
    {
        private readonly IAccountPartnerRepository _accountPartnerRepo;
        private readonly AddressService _addressService;
        private readonly CompanyService _companyService;
        public PartnerService(IUnitOfWork unitOfWork, JwtAuthen jwtAuthen, AddressService addressService, CompanyService companyService) : base(unitOfWork, jwtAuthen)
        {
            _accountPartnerRepo = _unitOfWork.AccountPartnerRepo;
            _addressService = addressService;
            _companyService = companyService;
        }

        public List<AccountPartner> GetAll()
        {
            var accountPartners = _accountPartnerRepo.GetAll().ToList();
            if (accountPartners != null)
            {
                return accountPartners;
            }
            return new List<AccountPartner>(0);
        }

        new public AccountPartner? GetById(Guid id)
        {
            return _accountPartnerRepo.GetById(id);
        }

        public AccountPartner? GetByUserName(string userName)
        {
            return _accountPartnerRepo.GetByUserName(userName);
        }

        public void AddAccount(AccountPartner accountPartner)
        {
            _accountPartnerRepo.Add(accountPartner);
        }

        public void DeleteAccount(AccountPartner accountPartner)
        {
            _accountPartnerRepo.Delete(accountPartner);
        }

        public ValidationSummary ValidatePartnerRegistrationRecord(PartnerRegistrationRecord? partnerRegistrationRecord)
        {
            if (partnerRegistrationRecord == null)
            {
                return new ValidationSummary(false, "Registration infomation is required.");
            }
            var validator = new PartnerRegistrationRecordValidator(_addressService, _companyService);
            var result = validator.Validate(partnerRegistrationRecord);
            return result.GetSummary();
        }

        public AccountPartner CreateAccount(PartnerRegistrationRecord partnerRegistrationRecord)
        {
            var Account = partnerRegistrationRecord.Account!;
            var accountPartner = new AccountPartner
            {
                UserName = Account!.UserName!.ToLower(),
                Password = Account!.Password!.ToHashSHA256(),
                Name = Account!.Name!.ToTitleCase(),
                Gender = Account!.Gender!.Value,
                DateOfBirth = Account!.DateOfBirth!.ToDateOnly(),
                Address = new Address
                {
                    WardId = Account!.Address!.WardId,
                    Street = Account!.Address!.Street!.ToTitleCase()
                },
                PartnerType = partnerRegistrationRecord!.PartnerType!.Value,
                IsVerified = false,
                CreatedAt = DateTime.Now,
            };

            var companyRecord = partnerRegistrationRecord.Company;
            if (accountPartner.PartnerType == PartnerType.Company)
            {
                accountPartner.Company = new Company
                {
                    Name = companyRecord!.Name!.ToTitleCase(),
                    BusinessCode = companyRecord!.BusinessCode!.ToUpper(),
                    Address = new Address
                    {
                        Street = companyRecord.Address!.Street!.ToTitleCase(),
                        WardId = companyRecord!.Address!.WardId,
                    }
                };
            }
            _accountPartnerRepo.Add(accountPartner);
            return accountPartner;
        }
        public ValidationSummary ValidatePartnerUpdateRecord(AccountPartner currentPartner, PartnerUpdateRecord? partnerUpdateRecord)
        {
            if (partnerUpdateRecord == null)
            {
                return new ValidationSummary(false, "Registration infomation is required.");
            }
            var validator = new PartnerUpdateRecordValidator(currentPartner, _addressService, _companyService);
            var result = validator.Validate(partnerUpdateRecord);
            return result.GetSummary();
        }

        public AccountPartner UpdateAccount(AccountPartner accountPartner, PartnerUpdateRecord partnerUpdateRecord)
        {
            var Account = partnerUpdateRecord.AccountUpdate!;
            accountPartner.Name = Account!.Name!.ToTitleCase();
            accountPartner.Gender = Account!.Gender!.Value;
            accountPartner.DateOfBirth = Account!.DateOfBirth!.ToDateOnly();
            accountPartner.Address = new Address
            {
                WardId = Account!.Address!.WardId,
                Street = Account!.Address!.Street!.ToTitleCase()
            };
            accountPartner.PartnerType = partnerUpdateRecord!.PartnerType!.Value;
            _companyService.Remove(accountPartner.Company);
            accountPartner.Company = null;
            if (accountPartner.PartnerType == PartnerType.Company)
            {
                var companyRecord = partnerUpdateRecord.Company;
                accountPartner.Company = new Company
                {
                    Name = companyRecord!.Name!.ToTitleCase(),
                    BusinessCode = companyRecord!.BusinessCode!.ToUpper(),
                    Address = new Address
                    {
                        Street = companyRecord.Address!.Street!.ToTitleCase(),
                        WardId = companyRecord!.Address!.WardId,
                    }
                };
            }
            _accountPartnerRepo.Update(accountPartner);
            return accountPartner;
        }

        #region Count partner
        
        public int CountAll()
        {
            return _accountPartnerRepo.GetAll().Count();
        }

        public int CountVerified()
        {
            return _accountPartnerRepo.GetAll().Where(p => p.IsVerified).Count();
        }

        #endregion

        #region Company

        public ValidationSummary ValidateCompanyRecord(CompanyRecord? companyRecord)
        {
            if (companyRecord == null)
            {
                return new ValidationSummary(false, "Company info is required.");
            }
            var validator = new CompanyRecordValidator(_addressService, _companyService);
            var result = validator.Validate(companyRecord);
            return result.GetSummary();
        }

        public Company CreateCompany(CompanyRecord companyRecord)
        {
            return new Company
            {
                Name = companyRecord!.Name!.ToTitleCase(),
                BusinessCode = companyRecord!.BusinessCode!.ToUpper(),
                Address = new Address
                {
                    WardId = companyRecord!.Address!.WardId,
                    Street = companyRecord.Address.Street
                }
            };
        }
        #endregion

        #region store
        public Store AddStore(Guid accountPartnerId, StoreRecord storeRecord)
        {
            var store = new Store
            {
                Name = storeRecord!.Name!.ToTitleCase(),
                Description = storeRecord.Description!,
                Address = new Address
                {
                    Street = storeRecord.Address!.Street,
                    WardId = storeRecord!.Address!.WardId,
                },
                OpenTime = storeRecord.OpenTime!.ToTimeOnly(),
                CloseTime = storeRecord.CloseTime!.ToTimeOnly(),
                CreatedAt = DateTime.Now,
                IsEnable = storeRecord.IsEnable!.Value,
            };
            var accountPartner = _accountPartnerRepo.GetById(accountPartnerId);
            if (accountPartner!.Store == null)
            {
                accountPartner!.Store = store;
                _accountPartnerRepo.Update(accountPartner);
            }
            return store;
        }

        public Store AddStore(AccountPartner accountPartner, StoreRecord storeRecord)
        {
            var store = new Store
            {
                Name = storeRecord.Name!.ToTitleCase(),
                Description = storeRecord.Description!,
                Address = new Address
                {
                    Street = storeRecord.Address!.Street,
                    WardId = storeRecord!.Address!.WardId,
                },
                OpenTime = storeRecord.OpenTime!.ToTimeOnly(), 
                CloseTime = storeRecord.CloseTime!.ToTimeOnly(), 
                CreatedAt = DateTime.Now,
                IsEnable = storeRecord.IsEnable!.Value,
            };
            if (accountPartner!.Store == null)
            {
                accountPartner!.Store = store;
                _accountPartnerRepo.Update(accountPartner);
            }
            return store;
        }
        #endregion
    }
}
