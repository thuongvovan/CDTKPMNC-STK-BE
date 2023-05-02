using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class AccountAdminRepository : IAccountAdminRepository
    {
        private readonly AppDbContext _dbContext;
        public AccountAdminRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<AccountAdmin> GetAll()
        {
            return _dbContext.AdminAccounts.ToList();
        }
        public AccountAdmin? GetById(Guid id)
        {
            return _dbContext.AdminAccounts.Find(id);
        }
        public AccountAdmin? GetByUserName(string account)
        {
            return _dbContext.AdminAccounts.SingleOrDefault(a => a.UserName == account);
        }
        public void Add(AccountAdmin adminAccount)
        { 
            if (adminAccount != null)
            {
                _dbContext.AdminAccounts.Add(adminAccount);
                _dbContext.SaveChanges();
            }
        }
        public void Delete(Guid id)
        {
            var adminAccount = _dbContext.AdminAccounts.Find(id);
            if (adminAccount != null)
            {
                _dbContext.AdminAccounts.Remove(adminAccount);
            }
        }
        public void Delete(AccountAdmin adminAccount)
        {
            if (adminAccount != null)
            {
                _dbContext.AdminAccounts.Remove(adminAccount);
            }
        }
        public void Update(AccountAdmin adminAccount)
        {
            _dbContext.AdminAccounts.Update(adminAccount);
            _dbContext.SaveChanges();
        }
    }
}
