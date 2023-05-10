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

        public void Approve(Store store)
        {
            if (store != null)
            {
                store.IsApproved = true;
                store.ApprovedAt = DateTime.Now;
                _dbContext.SaveChanges();
            }
        }

        public void Reject(Store store)
        {
            store.IsApproved = false;
            _dbContext.SaveChanges();
        }

        public void Delete(Store store)
        {
            if (store != null)
            {
                _dbContext.Stores.Remove(store);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteById(Guid id)
        {
            Store? store = GetById(id);
            if (store != null)
            {
                Delete(store);
            }
        }

        public void Disable(Store store)
        {
            if(store != null)
            {
                store.IsEnable = false;
                _dbContext.SaveChanges();
            }    
        }

        public void Enable(Store store)
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

        public Store? GetById(Guid Id)
        {
            return _dbContext.Stores.Find(Id);
        }

        public Store? GetByName(string name)
        {
            return _dbContext.Stores.SingleOrDefault(s => s.Name == name);
        }

        
    }
}
