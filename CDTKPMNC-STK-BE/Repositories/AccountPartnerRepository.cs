using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public class AccountPartnerRepository : IAccountPartnerRepository
    {
        private readonly AppDbContext _dbContext;
        public AccountPartnerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(AccountPartner account)
        {
            if (account != null)
            {
                _dbContext.AccountPartners.Add(account);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            AccountPartner? account = GetById(id);
            if (account != null)
            {
                _dbContext.AccountPartners.Remove(account);
            }
        }

        public void Delete(AccountPartner account)
        {
            if (account != null)
            {
                _dbContext.AccountPartners.Remove(account);
            }
        }

        public List<AccountPartner> GetAll()
        {
            return _dbContext.AccountPartners.ToList();
        }

        public AccountPartner? GetById(Guid id)
        {
            return _dbContext.AccountPartners.Find(id);
        }

        public AccountPartner? GetByUserName(string account)
        {
            return _dbContext.AccountPartners.SingleOrDefault(x => x.UserName == account.ToLower());
        }

        public void Update(AccountPartner account)
        {
            if (account != null)
            {
                _dbContext.AccountPartners.Update(account);
                _dbContext.SaveChanges();
            }
        }
    }
}
