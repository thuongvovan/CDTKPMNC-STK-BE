using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class EndUserAccountRepository : IEndUserAccountRepository
    {
        private readonly AppDBContext _context;
        public EndUserAccountRepository(AppDBContext dbContext)
        {
            _context = dbContext;
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

        public void Add(EndUserAccount endUser)
        {
            _context.Add(endUser);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var endUser = _context.EndUserAccounts.Find(id);
            if (endUser != null)
            {
                _context.EndUserAccounts.Remove(endUser);
                _context.SaveChanges();
            }
        }

        public void Delete(EndUserAccount endUser)
        {
            if (endUser != null)
            {
                _context.EndUserAccounts.Remove(endUser);
                _context.SaveChanges();
            }
        }

        public void Update(EndUserAccount endUser)
        {

             _context.EndUserAccounts.Update(endUser);
             _context.SaveChanges();
        }
    }
}
