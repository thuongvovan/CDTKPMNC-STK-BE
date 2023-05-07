using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IAccountEndUserRepository
    {
        List<AccountEndUser> GetAll();
        AccountEndUser? GetById(Guid id);
        AccountEndUser? GetByUserName(string account);
        void Add(AccountEndUser endUser);
        void Delete(Guid id);
        void Delete(AccountEndUser endUser);
        void Update(AccountEndUser endUser);


    }
}
