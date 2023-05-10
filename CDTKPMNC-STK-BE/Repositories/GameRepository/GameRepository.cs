using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.Repositories
{
    public class GameRepository : IGameRepository
    {
        readonly AppDbContext _dbContext;
        public GameRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public void Add(GameInfo gameInfo)
        {
            if (gameInfo != null)
            {
                var game = new Game
                {
                    Name = gameInfo.Name,
                    Description = gameInfo.Description,
                    Instruction = gameInfo.Instruction,
                    IsEnable = gameInfo.IsEnable,
                    CreatedAt = DateTime.Now
                };
                Add(game);
                _dbContext.SaveChanges();
            }
        }

        public void Add(Game game)
        {
            if(game != null)
            {
                _dbContext.Games.Add(game);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Game game)
        {
            if (game != null)
            {
                _dbContext.Games.Remove(game);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteById(Guid id)
        {
            var game = _dbContext.Games.Find(id);
            if (game != null)
            {
                _dbContext.Games.Remove(game);
                _dbContext.SaveChanges();
            }
        }

        public void Disable(Game game)
        {
            if (game != null)
            {
                game.IsEnable = false;
                _dbContext.SaveChanges();
            }
        }

        public void Enable(Game game)
        {
            if (game != null)
            {
                game.IsEnable = true;
                _dbContext.SaveChanges();
            }
        }

        public List<Game> GetAll()
        {
            return _dbContext.Games.ToList();
        }

        public List<Game> GetAvailable()
        {
            return _dbContext.Games.Where(g => g.IsEnable).ToList();
        }

        public Game? GetById(Guid id)
        {
            return _dbContext.Games.Find(id);
        }

        public Game? GetByName(string name)
        {
            return _dbContext.Games.SingleOrDefault(g => g.Name == name);
        }

        public void Update(Game game)
        {
            if (game != null)
            {
                _dbContext.Games.Update(game);
                _dbContext.SaveChanges();
            }
        }
    }
}
