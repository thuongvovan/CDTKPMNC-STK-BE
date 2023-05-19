﻿using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository;
using CDTKPMNC_STK_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IAccountRepository<Account> _accountRepository;
        private IAccountEndUserRepository _accountEndUserRepository;
        private IAccountAdminRepository _accountAdminRepository;
        public IAccountPartnerRepository _accountPartnerRepository;
        public IProvinceRepository _provinceRepository;
        public IDistrictRepository _districtRepository;
        public IWardRepository _wardRepository;
        public ICompanyRepository _companyRepository;
        public IStoreRepository _storeRepository;
        public IGameRepository _gameRepository;
        public IProductCategoryRepository _productCategoryRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _accountRepository = new AccountRepository<Account>(_context);
            _accountEndUserRepository = new AccountEndUserRepository(_context);
            _accountAdminRepository = new AccountAdminRepository(_context);
            _accountPartnerRepository = new AccountPartnerRepository(_context);
            _provinceRepository = new ProvinceRepository(_context);
            _districtRepository = new DistrictRepository(_context);
            _wardRepository = new WardRepository(_context);
            _companyRepository = new CompanyRepository(_context);
            _storeRepository = new StoreRepository(_context);
            _gameRepository = new GameRepository(_context);
            _productCategoryRepository = new ProductCategoryRepository(_context);
        }

        public IAccountRepository<Account> AccountRepo
        {
            get
            {
                _accountRepository ??= new AccountRepository<Account>(_context);
                return _accountRepository;
            }
        }
        public IAccountEndUserRepository AccountEndUserRepo
        {
            get
            {
                _accountEndUserRepository ??= new AccountEndUserRepository(_context);
                return _accountEndUserRepository;
            }
        }
        public IProvinceRepository ProvinceRepo
        {
            get
            {
                _provinceRepository ??= new ProvinceRepository(_context);
                return _provinceRepository;
            }
        }
        public IDistrictRepository DistrictRepo
        {
            get
            {
                _districtRepository ??= new DistrictRepository(_context);
                return _districtRepository;
            }
        }
        public IWardRepository WardRepo
        {
            get
            {
                _wardRepository ??= new WardRepository(_context);
                return _wardRepository;
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
