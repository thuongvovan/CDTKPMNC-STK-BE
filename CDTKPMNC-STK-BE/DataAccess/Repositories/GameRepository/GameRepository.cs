using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class GameRepository : CommonRepository<Game>, IGameRepository
    {
        public GameRepository(AppDbContext dbContext) : base(dbContext) { }

        public void Add(GameRecord gameInfo)
        {
            if (gameInfo != null)
            {
                var game = new Game
                {
                    Name = gameInfo.Name!,
                    Description = gameInfo.Description!,
                    Instruction = gameInfo.Instruction!,
                    IsEnable = gameInfo.IsEnable!.Value,
                    CreatedAt = DateTime.Now
                };
                Add(game);
                Save();
            }
        }
        public void Disable(Game game)
        {
            if (game != null)
            {
                game.IsEnable = false;
                Save();
            }
        }
        public void Enable(Game game)
        {
            if (game != null)
            {
                game.IsEnable = true;
                Save();
            }
        }
        public List<Game> GetAvailable()
        {
            return _table.Where(g => g.IsEnable).ToList();
        }
        public Game? GetByName(string name)
        {
            return _table.SingleOrDefault(g => g.Name == name);
        }
        public void Update(Game game, GameRecord gameInfo)
        {
            if (game != null && gameInfo != null)
            {
                game.Name = gameInfo.Name!;
                game.Description = gameInfo.Description!;
                game.Instruction = gameInfo.Instruction!;
                game.IsEnable = gameInfo.IsEnable!.Value;
                Update(game);
                Save();
            }
        }
    }
}
