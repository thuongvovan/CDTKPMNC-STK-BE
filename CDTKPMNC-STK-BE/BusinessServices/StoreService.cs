using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using System.Security.Cryptography.X509Certificates;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class StoreService : CommonService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly AddressService _addressService;

        public StoreService(IUnitOfWork unitOfWork, AddressService addressService) : base(unitOfWork)
        {
            _storeRepo = _unitOfWork.StoreRepo;
            _addressService = addressService;
        }

        public List<Store> GetAll()
        {
            var stores = _storeRepo.GetAll();
            if (stores == null)
            {
                return new List<Store>(0);
            }
            return stores.ToList();
        }

        public List<Store> GetApproved()
        {
            var stores = _storeRepo.GetApproved();
            if (stores == null)
            {
                return new List<Store>(0);
            }
            return stores;
        }

        public List<Store> GetRejected()
        {
            var stores = _storeRepo.GetRejected();
            if (stores == null)
            {
                return new List<Store>(0);
            }
            return stores;
        }

        public List<Store> GetNeedApproval()
        {
            var stores = _storeRepo.GetNeedApproval();
            if (stores == null)
            {
                return new List<Store>(0);
            }
            return stores;
        }

        public Store? GetById(Guid storeId)
        {
            var store = _storeRepo.GetById(storeId);
            return store;
        }

        public void Approve(Store store)
        {
            store.ApprovedAt = DateTime.Now;
            store.IsApproved = true;
            _storeRepo.Update(store);
        }

        public void Reject(Store store)
        {
            store.IsApproved = false;
            _storeRepo.Update(store);
        }

        public void Disable(Store store)
        {
            store.IsEnable = false;
            _storeRepo.Update(store);
        }

        public void Enable(Store store)
        {
            store.IsEnable = true;
            _storeRepo.Update(store);
        }

        public ValidationSummary ValidateStoreRecord(StoreRecord storeRecord)
        {
            if (storeRecord == null)
            {
                return new ValidationSummary(false, "Store info is required.");
            }
            var validator = new StoreRecordValidator(_addressService);
            var result = validator.Validate(storeRecord);
            return result.GetSummary();
        }

        public bool VerifyStoreRecord(StoreRecord storeRecord, Guid accountPartnerId)
        {
            var currentStore = _storeRepo.GetByName(storeRecord.Name!);
            if (currentStore != null) return true;
            currentStore = _storeRepo.GetById(accountPartnerId);
            if (currentStore != null) return true;
            return false;
        }

        public Store AddStore(StoreRecord storeRecord, Guid accountPartnerId)
        {
            var store = new Store
            {
                Id = accountPartnerId,
                Name = storeRecord!.Name!.ToTitleCase(),
                Description = storeRecord!.Description!,
                Address = new Address
                {
                    Street = storeRecord.Address!.Street,
                    WardId = storeRecord!.Address!.WardId,
                },
                OpenTime = storeRecord.OpenTime!.ToTimeOnly(),
                CloseTime = storeRecord.CloseTime!.ToTimeOnly(),
                CreatedAt = DateTime.Now,
                IsEnable = storeRecord.IsEnable!.Value,
            };
            _storeRepo.Add(store);
            return store;
        }

        public Store UpdateStore(Store store, StoreRecord storeRecord)
        {
            store.Name = storeRecord!.Name!.ToTitleCase();
            store.Description = storeRecord!.Description!;
            store.Address.Street = storeRecord.Address!.Street;
            store.Address.WardId = storeRecord!.Address!.WardId;
            store.OpenTime = storeRecord.OpenTime!.ToTimeOnly();
            store.CloseTime = storeRecord.CloseTime!.ToTimeOnly();
            store.IsEnable = storeRecord.IsEnable!.Value;
            _storeRepo.Update(store);
            return store;
        }

        public Store? EnableStore(Guid accountPartnerId)
        {
            var store = _storeRepo.GetById(accountPartnerId);
            if (store != null)
            {
                store.IsEnable = true;
                _storeRepo.Update(store);
                return store;
            }
            return null;
        }

        public Store? DisableStore(Guid accountPartnerId)
        {
            var store = _storeRepo.GetById(accountPartnerId);
            if (store != null)
            {
                store.IsEnable = false;
                _storeRepo.Update(store);
                return store;
            }
            return null;
        }

    }
}
