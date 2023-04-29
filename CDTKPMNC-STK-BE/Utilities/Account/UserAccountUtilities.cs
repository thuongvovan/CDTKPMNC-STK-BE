using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Utilities.Account
{
    static public class UserAccountUtilities
    {
        static public EndUserAccount CreateUserAccount(this RegisteredAccount registeredAccount)
        {
            var userAccount = new EndUserAccount
            {
                Account = registeredAccount.Account.ToLower(),
                Password = registeredAccount.Password.ToHashSHA256(),
                Name = registeredAccount.Name.ToTitleCase(),
                Gender = registeredAccount.Gender,
                DateOfBirth = new DateTime(registeredAccount.BirthYear, registeredAccount.BirthMonth, registeredAccount.BirthDate),
            };
            return userAccount;
        }
    }
}
