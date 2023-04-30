using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        private IEndUserAccountRepository _endUserAccountRepository;
        public UnitOfWork(AppDBContext context) 
        {  
            _context = context;
            _endUserAccountRepository = new EndUserAccountRepository(_context);

        }
        public IEndUserAccountRepository EndUserAccountRepository
        {
            get
            {
                _endUserAccountRepository ??= new EndUserAccountRepository(_context);
                return _endUserAccountRepository;
            }
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
