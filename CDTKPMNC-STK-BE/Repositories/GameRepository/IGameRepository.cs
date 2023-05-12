using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface IGameRepository
    {
            void Add(Game game);
            void Add(GameInfo gameInfo);
            List<Game> GetAll();
            List<Game> GetAvailable();
            Game? GetById(Guid id);
            Game? GetByName(string name);
            void Delete(Game game);
            void DeleteById(Guid id);
            void Enable(Game game);
            void Disable(Game game);
            void Update(Game game);
            void Update(Game game, GameInfo gameInfo);
    }
}
