using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class ProductCategoryRepository : CommonRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(AppDbContext dbContext) : base(dbContext) { }
        public void Add(ProductCategoryRecord categoryInfo)
        {
            if (categoryInfo != null)
            {
                var productCategory = new ProductCategory
                {
                    Name = categoryInfo.Name!,
                    Description = categoryInfo.Description!,
                    IsEnable = categoryInfo.IsEnable!.Value,
                    CreatedAt = DateTime.Now,
                };
                _table.Add(productCategory);
                Save();
            }
        }
        public ProductCategory? GetByName(string name)
        {
            return _table.SingleOrDefault(c => c.Name.ToLower() == name.ToLower());
        }
        public List<ProductCategory> GetAvalible()
        {
            return _table.Where(c => c.IsEnable).ToList();
        }
        public void Update(ProductCategory category, ProductCategoryRecord productCategoryInfo)
        {
            category.Name = productCategoryInfo.Name!;
            category.Description = productCategoryInfo.Description!;
            category.IsEnable = productCategoryInfo.IsEnable!.Value;
            _table.Update(category);
            Save();
        }
        public void Enable(ProductCategory category)
        {
            category.IsEnable = true;
            Save();
        }
        public void Disable(ProductCategory category)
        {
            category.IsEnable = false;
            Save();
        }
    }
}
