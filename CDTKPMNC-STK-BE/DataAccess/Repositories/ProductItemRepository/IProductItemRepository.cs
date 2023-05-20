using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface IProductItemRepository : ICommonRepository<ProductItem>
    {
        List<ProductItem> GetAvalible();
        List<ProductItem> GetDisabled();
        List<ProductItem> GetAvalibleByStore(Guid storeId);
        List<ProductItem> GetDisabledByStore(Guid storeId);
        ProductItem? GetByName(string name);
        ProductItem? GetByNameByStore(string name, Guid storeId);
        List<ProductItem> GetAllByStore(Guid storeId);
    }
}
