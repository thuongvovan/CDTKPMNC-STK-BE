using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public class StoreRepository: IStoreRepository
    {
        private readonly AppDbContext _dbContext;
        public StoreRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Store store)
        {
            if (store != null)
            {
                _dbContext.Stores.Add(store);
                _dbContext.SaveChanges();
            }
        }

        public void ApproveStore(Store store)
        {
            if (store != null)
            {
                store.IsApproved = true;
                store.ApprovedAt = DateTime.Now;
                _dbContext.SaveChanges();
            }
        }

        public void RejectStore(Store store)
        {
            store.IsApproved = false;
            _dbContext.SaveChanges();
        }

        public void DeleteStore(Store store)
        {
            if (store != null)
            {
                _dbContext.Stores.Remove(store);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteStoreById(Guid id)
        {
            Store? store = GetStoreById(id);
            if (store != null)
            {
                DeleteStore(store);
            }
        }

        public void DisableStore(Store store)
        {
            if(store != null)
            {
                store.IsEnable = false;
                _dbContext.SaveChanges();
            }    
        }

        public void EnableStore(Store store)
        {
            if (store != null)
            {
                store.IsEnable = true;
                _dbContext.SaveChanges();
            }
        }

        public List<Store> GetAll()
        {
            return _dbContext.Stores.ToList();
        }

        public List<Store> GetApproved()
        {
            return _dbContext.Stores.Where(s => s.IsApproved ?? false).ToList();
        }

        public List<Store> GetRejected()
        {
            return _dbContext.Stores.Where(s => !s.IsApproved ?? false).ToList();
        }

        public List<Store> GetNeedApproval()
        {
            return _dbContext.Stores.Where(s => s.IsApproved == null).ToList();
        }

        public Store? GetStoreById(Guid Id)
        {
            return _dbContext.Stores.Find(Id);
        }

        public Store? GetStoreByName(string name)
        {
            return _dbContext.Stores.SingleOrDefault(s => s.Name == name);
        }

        
    }
}
