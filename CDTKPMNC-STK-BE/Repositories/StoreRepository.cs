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

        public void ActiveStore(Store store)
        {
            if (store != null)
            {
                store.IsApproved = true;
                store.ApprovedAt = DateTime.Now;
                _dbContext.SaveChanges();
            }
        }

        public void AddStore(Store store)
        {
            if (store != null)
            {
                _dbContext.Stores.Add(store);
                _dbContext.SaveChanges();
            }
        }

        public void DeactiveStore(Store store)
        {
            if (store != null)
            {
                store.IsApproved = false;
                _dbContext.SaveChanges();
            }
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

        public List<Store> GetAllCompanys()
        {
            return _dbContext.Stores.ToList();
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
