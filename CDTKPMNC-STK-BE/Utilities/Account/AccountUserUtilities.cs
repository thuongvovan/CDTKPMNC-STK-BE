using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Repositories;
using System.Runtime.CompilerServices;

namespace CDTKPMNC_STK_BE.Utilities.AccountUtils
{
    static public class AccountUserUtilities
    {
        static public DateOnly  ToDateOnly(this Date date)
        {
            return new DateOnly(date.Year, date.Month, date.Day);
        }

        static public AccountEndUser CreateUserAccount(this EndUserRegistrationInfo registeredAccount, AddressWard ward)
        {
            var userAccount = new AccountEndUser
            {
                UserName = registeredAccount.UserName.ToLower(),
                Password = registeredAccount.Password.ToHashSHA256(),
                Name = registeredAccount.Name.ToTitleCase(),
                Gender = registeredAccount.Gender,
                DateOfBirth = registeredAccount.BirthDate.ToDateOnly(),
                CreatedAt = DateTime.Now,
                Address = new Address
                {
                    Ward = ward,
                    District = ward!.District,
                    Province = ward.Province,
                    Street = registeredAccount.Address.Street.ToTitleCase()
                }
            };
            return userAccount;
        }

        static public AccountPartner CreateUserAccount(this PartnerRegistrationInfo registeredAccount, AddressWard ward)
        {
            var userAccount = new AccountPartner
            {
                UserName = registeredAccount.UserName.ToLower(),
                Password = registeredAccount.Password.ToHashSHA256(),
                Name = registeredAccount.Name.ToTitleCase(),
                Gender = registeredAccount.Gender,
                DateOfBirth = registeredAccount.BirthDate.ToDateOnly(), // DateOnly.FromDateTime( new DateTime(registeredAccount.BirthYear, registeredAccount.BirthMonth, registeredAccount.BirthDate)),
                PertnerType = registeredAccount.Type,
                CreatedAt = DateTime.Now,
                Address = new Address 
                    { 
                        Province = ward!.Province,
                        District = ward.District,
                        Ward = ward,
                        Street =registeredAccount.Address.Street.ToTitleCase()
                    }
            };
            return userAccount;
        }

        static public Company CreateCompany(this CompanyRegistrationInfo registeredCompany, AddressWard ward)
        {
            var company = new Company
            {
                Name = registeredCompany.Name.ToTitleCase(),
                BusinessCode = registeredCompany.BusinessCode.ToUpper(),
                Address = new Address
                {
                    Ward = ward,
                    District = ward!.District,
                    Province = ward.Province,
                    Street = registeredCompany.Address.Street.ToTitleCase()
                }
            };
            return company;
        }

        static public Store CreateStore(this StoreRegistrationInfo registeredStore, AddressWard ward)
        {
            var store = new Store
            {
                Name = registeredStore.Name.ToTitleCase(),
                Description = registeredStore.Description,
                Address = new Address
                {
                    Ward = ward,
                    District = ward!.District,
                    Province = ward.Province,
                    Street = registeredStore.Address.Street.ToTitleCase()
                },
                OpenTime = new TimeOnly(registeredStore.OpenTime.Hours, registeredStore.OpenTime.Minute),
                CloseTime = new TimeOnly(registeredStore.CloseTime.Hours, registeredStore.CloseTime.Minute),
                IsEnable = registeredStore.IsEnable,
                CreatedAt = DateTime.Now,
            };
            return store;
        }

    }
}
