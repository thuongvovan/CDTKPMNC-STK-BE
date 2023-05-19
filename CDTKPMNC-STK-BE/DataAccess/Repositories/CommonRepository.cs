using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _table;
        public CommonRepository(AppDbContext context)
        {
            _dbContext = context;
            _table = _dbContext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _table.ToList();
        }
        public T? GetById(object id)
        {
            return _table.Find(id);
        }
        public void Add(T obj)
        {
            if (obj != null)
            {
                _table.Add(obj);
                Save();
            }
        }
        public void Update(T obj)
        {
            if (obj != null)
            {
                _table.Update(obj);
                Save();
            }
        }
        public void Delete(object id)
        {
            T? obj = _table.Find(id);
            if (obj != null)
            {
                Delete(obj);
                Save();
            }
        }

        public void Delete(T obj)
        {
            if (obj != null)
            {
                _table.Remove(obj);
                Save();
            }
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
