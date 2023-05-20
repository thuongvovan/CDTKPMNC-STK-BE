using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class GameRepository : CommonRepository<Game>, IGameRepository
    {
        public GameRepository(AppDbContext dbContext) : base(dbContext) { }
        public List<Game> GetAvailable()
        {
            return _table.Where(g => g.IsEnable).ToList();
        }
        public Game? GetByName(string name)
        {
            return _table.SingleOrDefault(g => g.Name.ToLower() == name.ToLower());
        }
    }
}
