using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Repositories;
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
        public IGameRepository _gameRepository;
        public IProductCategoryRepository _productCategoryRepository;
        public UnitOfWork(AppDbContext context) 
        {  
            _context = context;
            _accountEndUserRepository = new AccountEndUserRepository(_context);
            _accountAdminRepository = new AccountAdminRepository(_context);
            _accountPartnerRepository = new AccountPartnerRepository(_context);
            _addressRepository = new AddressRepository(_context);
            _companyRepository = new CompanyRepository(_context);
            _storeRepository = new StoreRepository(_context);
            _gameRepository = new GameRepository(_context);
            _productCategoryRepository = new ProductCategoryRepository(_context);
        }
        public IAccountEndUserRepository AccountEndUserRepo
        {
            get
            {
                _accountEndUserRepository ??= new AccountEndUserRepository(_context);
                return _accountEndUserRepository;
            }
        }
        public IAddressRepository AddressRepo
        { 
            get
            {
                _addressRepository ??= new AddressRepository(_context);
                return _addressRepository;
            } 
        }

        public IAccountPartnerRepository AccountPartnerRepo
        {
            get
            {
                _accountPartnerRepository ??= new AccountPartnerRepository(_context);
                return _accountPartnerRepository;
            }
        }

        public IAccountAdminRepository AccountAdminRepo
        {
            get
            {
                _accountAdminRepository ??= new AccountAdminRepository(_context);
                return _accountAdminRepository;
            }
        }

        public ICompanyRepository CompanyRepo
        {
            get
            {
                _companyRepository ??= new CompanyRepository(_context);
                return _companyRepository;
            }
        }

        public IStoreRepository StoreRepo
        {
            get
            {
                _storeRepository ??= new StoreRepository(_context);
                return _storeRepository;
            }
        }

        public IGameRepository GameRepo
        {
            get
            {
                _gameRepository ??= new GameRepository(_context);
                return _gameRepository;
            }
        }

        public IProductCategoryRepository ProductCategoryRepo
        {
            get
            {
                _productCategoryRepository ??= new ProductCategoryRepository(_context);
                return _productCategoryRepository;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
