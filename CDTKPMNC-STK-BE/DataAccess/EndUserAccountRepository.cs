using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class EndUserAccountRepository : IRepository<EndUserAccount, Guid>
    {
        private readonly AppDBContext _context;
        public EndUserAccountRepository(AppDBContext dbContext)
        {
            _context = dbContext;
        }

        public void Add(EndUserAccount model)
        {
            _context.Add(model);
            _context.SaveChangesAsync();
        }

        public void Delete(Guid id)
        {
            var endUserAccount = _context.EndUserAccounts.Find(id);
            if (endUserAccount != null)
            {
                _context.EndUserAccounts.Remove(endUserAccount);
            }
        }

        public void Delete(EndUserAccount model)
        {
            if (model != null)
            {
                _context.EndUserAccounts.Remove(model);
            }
        }

        public void Delete(Guid id, EndUserAccount newModel)
        {
            throw new NotImplementedException();
        }

        public List<EndUserAccount> GetAll()
        {
            return _context.EndUserAccounts.ToList();
        }

        public EndUserAccount? GetById(Guid id)
        {
            return _context.EndUserAccounts.SingleOrDefault(u => u.Id == id);
        }

        public EndUserAccount? GetByAccount(string account)
        {
            return _context.EndUserAccounts.SingleOrDefault(u => u.Account == account);
        }

        public void Update(Guid id, EndUserAccount newModel)
        {
            var account = _context.EndUserAccounts.SingleOrDefault(u => u.Id == id);
            if (account != null)
            {
                account.Account = newModel.Account;
                account.Password = newModel.Password;
                account.NewPassword = newModel.NewPassword;
                account.Name = newModel.Name;
                account.Gender = newModel.Gender;
                account.DateOfBirth = newModel.DateOfBirth;
                account.IsVerified = newModel.IsVerified;
                _context.SaveChangesAsync();
            }
        }
    }
}
