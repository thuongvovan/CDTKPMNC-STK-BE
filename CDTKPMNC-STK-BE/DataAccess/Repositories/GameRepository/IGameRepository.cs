using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface IGameRepository : ICommonRepository<Game>
    {
        List<Game> GetAvailable();
        Game? GetByName(string name);
    }
}
