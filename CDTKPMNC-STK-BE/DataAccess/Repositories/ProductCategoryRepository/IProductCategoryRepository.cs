using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface IProductCategoryRepository : ICommonRepository<ProductCategory>
    {
        List<ProductCategory> GetAvalible();
        List<ProductCategory> GetDisabled();
        ProductCategory? GetByName(string name);
    }
}
