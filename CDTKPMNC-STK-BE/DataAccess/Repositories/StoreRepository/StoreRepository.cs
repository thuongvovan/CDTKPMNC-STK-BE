using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class StoreRepository : CommonRepository<Store>, IStoreRepository
    {
        public StoreRepository(AppDbContext context) : base(context) { }
        public List<Store>? GetApproved()
        {
            return _table.Where(s => s.IsApproved ?? false).ToList();
        }

        public List<Store>? GetRejected()
        {
            return _table.Where(s => !s.IsApproved ?? false).ToList();
        }

        public List<Store>? GetNeedApproval()
        {
            return _table.Where(s => s.IsApproved == null).ToList();
        }

        public Store? GetByName(string name)
        {
            return _table.SingleOrDefault(s => s.Name.ToLower() == name.ToLower());
        }
    }
}
