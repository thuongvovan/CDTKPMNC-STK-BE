using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class NoticationRepository : CommonRepository<Notication>, INoticationRepository
    {
        public NoticationRepository(AppDbContext context) : base(context)
        {
        }

        public List<Notication> GetByUser(Guid userId)
        {
            return _table.Where(n => n.AccountId == userId).ToList();
        }
    }
}
