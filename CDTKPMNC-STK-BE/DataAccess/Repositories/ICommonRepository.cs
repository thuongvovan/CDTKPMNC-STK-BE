namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface ICommonRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(object id);
        void Add(T obj);
        void Update(T obj);
        void Delete(object id);
        void Delete(T obj);
        void Save();
    }
}
