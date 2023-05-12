using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
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
            return _dbContext.AccountAdmins.ToList();
        }
        public AccountAdmin? GetById(Guid id)
        {
            return _dbContext.AccountAdmins.Find(id);
        }
        public AccountAdmin? GetByUserName(string account)
        {
            return _dbContext.AccountAdmins.SingleOrDefault(a => a.UserName == account.ToLower());
        }
        public void Add(AccountAdmin adminAccount)
        { 
            if (adminAccount != null)
            {
                _dbContext.AccountAdmins.Add(adminAccount);
                _dbContext.SaveChanges();
            }
        }
        public void Delete(Guid id)
        {
            var adminAccount = _dbContext.AccountAdmins.Find(id);
            if (adminAccount != null)
            {
                _dbContext.AccountAdmins.Remove(adminAccount);
            }
        }
        public void Delete(AccountAdmin adminAccount)
        {
            if (adminAccount != null)
            {
                _dbContext.AccountAdmins.Remove(adminAccount);
            }
        }
        public void Update(AccountAdmin adminAccount)
        {
            _dbContext.AccountAdmins.Update(adminAccount);
            _dbContext.SaveChanges();
        }
    }
}
