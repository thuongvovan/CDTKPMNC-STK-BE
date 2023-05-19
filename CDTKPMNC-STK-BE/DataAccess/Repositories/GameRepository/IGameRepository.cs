using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface IGameRepository : ICommonRepository<Game>
    {
        void Add(GameRecord gameInfo);
        List<Game> GetAvailable();
        Game? GetByName(string name);
        void Enable(Game game);
        void Disable(Game game);
        void Update(Game game, GameRecord gameInfo);
    }
}
