using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class ProductItemRepository : CommonRepository<ProductItem>, IProductItemRepository
    {
        public ProductItemRepository(AppDbContext dbContext) : base(dbContext) { }

        public List<ProductItem> GetAvalible()
        {
            return _table.Where(i => i.IsEnable).ToList();
        }

        public List<ProductItem> GetDisabled()
        {
            return _table.Where(i => !i.IsEnable).ToList();
        }

        public List<ProductItem> GetAvalibleByStore(Guid storeId)
        {
            return _table.Where(i => i.IsEnable && i.StoreId == storeId).ToList();
        }

        public List<ProductItem> GetDisabledByStore(Guid storeId)
        {
            return _table.Where(i => !i.IsEnable && i.StoreId == storeId).ToList();
        }

        public ProductItem? GetByName(string name)
        {
            return _table.FirstOrDefault(i => i.Name == name.ToTitleCase().Trim());
        }

        public ProductItem? GetByNameByStore(string name, Guid storeId) 
        {
            return _table.FirstOrDefault(i => i.Name == name.ToTitleCase().Trim() && i.StoreId == storeId);
        }

        public List<ProductItem> GetAllByStore(Guid storeId)
        {
            return _table.Where(i => i.StoreId == storeId).ToList();
        }
    }
}
