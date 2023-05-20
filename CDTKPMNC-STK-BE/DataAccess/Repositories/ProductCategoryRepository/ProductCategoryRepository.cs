using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class ProductCategoryRepository : CommonRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(AppDbContext dbContext) : base(dbContext) { }
        public ProductCategory? GetByName(string name)
        {
            return _table.SingleOrDefault(c => c.Name.ToLower() == name.ToLower());
        }
        public List<ProductCategory> GetAvalible()
        {
            return _table.Where(c => c.IsEnable).ToList();
        }

        public List<ProductCategory> GetDisabled()
        {
            return _table.Where(c => !c.IsEnable).ToList();
        }
    }
}
