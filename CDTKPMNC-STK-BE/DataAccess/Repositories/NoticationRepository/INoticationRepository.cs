using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface INoticationRepository : ICommonRepository<Notication>
    {
        List<Notication> GetByUser(Guid userId);
    }
}
