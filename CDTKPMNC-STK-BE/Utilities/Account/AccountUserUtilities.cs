using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Utilities.Account
{
    static public class AccountUserUtilities
    {
        static public AccountEndUser CreateUserAccount(this RegisteredAccount registeredAccount)
        {
            var userAccount = new AccountEndUser
            {
                UserName = registeredAccount.UserName.ToLower(),
                Password = registeredAccount.Password.ToHashSHA256(),
                Name = registeredAccount.Name.ToTitleCase(),
                Gender = registeredAccount.Gender,
                DateOfBirth = DateOnly.FromDateTime( new DateTime(registeredAccount.BirthYear, registeredAccount.BirthMonth, registeredAccount.BirthDate)),
            };
            return userAccount;
        }
    }
}
