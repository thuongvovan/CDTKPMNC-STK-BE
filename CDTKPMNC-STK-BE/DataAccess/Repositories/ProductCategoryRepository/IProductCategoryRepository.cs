using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface IProductCategoryRepository : ICommonRepository<ProductCategory>
    {
        List<ProductCategory> GetAvalible();
        ProductCategory? GetByName(string name);
        void Add(ProductCategoryRecord productCategoryRecord);
        void Update(ProductCategory category, ProductCategoryRecord productCategoryInfo);
        void Enable(ProductCategory category);
        void Disable(ProductCategory category);
    }
}
