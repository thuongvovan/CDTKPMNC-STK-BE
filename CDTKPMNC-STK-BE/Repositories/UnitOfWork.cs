using CDTKPMNC_STK_BE.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IAccountEndUserRepository _endUserAccountRepository;
        private IAccountAdminRepository _adminAccountRepository;
        public UnitOfWork(AppDbContext context) 
        {  
            _context = context;
            _endUserAccountRepository = new AccountEndUserRepository(_context);
            _adminAccountRepository = new AccountAdminRepository(_context);

        }
        public IAccountEndUserRepository EndUserAccountRepository
        {
            get
            {
                _endUserAccountRepository ??= new AccountEndUserRepository(_context);
                return _endUserAccountRepository;
            }
        }
        public IAccountAdminRepository AdminAccountRepository 
        { 
            get
            {
                _adminAccountRepository ??= new AccountAdminRepository(_context);
                return _adminAccountRepository;
            } 
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
