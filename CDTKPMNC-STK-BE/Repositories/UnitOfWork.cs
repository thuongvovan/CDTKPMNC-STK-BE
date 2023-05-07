using CDTKPMNC_STK_BE.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IAccountEndUserRepository _accountEndUserRepository;
        private IAccountAdminRepository _accountAdminRepository;
        public IAccountPartnerRepository _accountPartnerRepository;
        public IAddressRepository _addressRepository;
        public ICompanyRepository _companyRepository;
        public IStoreRepository _storeRepository;
        public UnitOfWork(AppDbContext context) 
        {  
            _context = context;
            _accountEndUserRepository = new AccountEndUserRepository(_context);
            _accountAdminRepository = new AccountAdminRepository(_context);
            _accountPartnerRepository = new AccountPartnerRepository(_context);
            _addressRepository = new AddressRepository(_context);
            _companyRepository = new CompanyRepository(_context);
            _storeRepository = new StoreRepository(_context);
        }
        public IAccountEndUserRepository AccountEndUserRepository
        {
            get
            {
                _accountEndUserRepository ??= new AccountEndUserRepository(_context);
                return _accountEndUserRepository;
            }
        }
        public IAddressRepository AddressRepository
        { 
            get
            {
                _addressRepository ??= new AddressRepository(_context);
                return _addressRepository;
            } 
        }

        public IAccountPartnerRepository AccountPartnerRepository
        {
            get
            {
                _accountPartnerRepository ??= new AccountPartnerRepository(_context);
                return _accountPartnerRepository;
            }
        }

        public IAccountAdminRepository AccountAdminRepository
        {
            get
            {
                _accountAdminRepository ??= new AccountAdminRepository(_context);
                return _accountAdminRepository;
            }
        }

        public ICompanyRepository CompanyRepository
        {
            get
            {
                _companyRepository ??= new CompanyRepository(_context);
                return _companyRepository;
            }
        }

        public IStoreRepository StoreRepository
        {
            get
            {
                _storeRepository ??= new StoreRepository(_context);
                return _storeRepository;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
