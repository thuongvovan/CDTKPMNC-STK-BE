using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IAccountAdminRepository
    {
        List<AccountAdmin> GetAll();
        AccountAdmin? GetById(Guid id);
        AccountAdmin? GetByUserName(string account);
        void Add(AccountAdmin adminAccount);
        void Delete(Guid id);
        void Delete(AccountAdmin adminAccount);
        void Update(AccountAdmin adminAccount);
    }
}
