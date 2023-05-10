using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IAccountPartnerRepository
    {
        List<AccountPartner> GetAll();
        AccountPartner? GetById(Guid id);
        AccountPartner? GetByUserName(string account);
        void Add(AccountPartner accountPartner);
        void Delete(Guid id);
        void Delete(AccountPartner accountPartner);
        void Update(AccountPartner accountPartner);
    }
}
