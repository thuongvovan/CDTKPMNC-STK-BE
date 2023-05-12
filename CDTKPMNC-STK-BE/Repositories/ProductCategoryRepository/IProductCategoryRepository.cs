
using CDTKPMNC_STK_BE.Models;
using static CDTKPMNC_STK_BE.Controllers.ProductCategoryController;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IProductCategoryRepository
    {
        List<ProductCategory> GetAll();
        List<ProductCategory> GetAvalible();
        ProductCategory? GetById(Guid productCategoryId);
        ProductCategory? GetByName(string name);
        void Add(ProductCategory category);
        void Add(ProductCategoryInfo categoryInfo);
        void Update(ProductCategory category);
        void Update(ProductCategory category, ProductCategoryInfo productCategoryInfo);
        void Delete(ProductCategory category);
        void Delete(Guid productCategoryId);
        void Enable(ProductCategory category);
        void Disable(ProductCategory category);
    }
}
