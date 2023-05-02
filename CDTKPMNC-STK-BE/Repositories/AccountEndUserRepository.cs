using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class AccountEndUserRepository : IAccountEndUserRepository
    {
        private readonly AppDbContext _context;
        public AccountEndUserRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        public List<AccountEndUser> GetAll()
        {
            return _context.AccountEndUsers.ToList();
        }

        public AccountEndUser? GetById(Guid id)
        {
            return _context.AccountEndUsers.SingleOrDefault(u => u.Id == id);
        }

        public AccountEndUser? GetByUserName(string account)
        {
            return _context.AccountEndUsers.SingleOrDefault(u => u.UserName == account);
        }

        public void Add(AccountEndUser endUser)
        {
            _context.AccountEndUsers.Add(endUser);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var endUser = _context.AccountEndUsers.Find(id);
            if (endUser != null)
            {
                _context.AccountEndUsers.Remove(endUser);
                _context.SaveChanges();
            }
        }

        public void Delete(AccountEndUser endUser)
        {
            if (endUser != null)
            {
                _context.AccountEndUsers.Remove(endUser);
                _context.SaveChanges();
            }
        }

        public void Update(AccountEndUser endUser)
        {

            _context.AccountEndUsers.Update(endUser);
            _context.SaveChanges();
        }
    }
}
