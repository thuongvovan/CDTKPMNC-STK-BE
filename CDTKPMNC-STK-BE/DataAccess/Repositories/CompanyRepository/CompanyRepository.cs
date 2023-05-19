using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public class CompanyRepository : CommonRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext dbContext) : base(dbContext) { }

        public Company? GetByBusinessCode(string code)
        {
            if (code != null)
            {
                return _table.SingleOrDefault(c => c.BusinessCode == code.ToUpper());
            }
            return null;
        }
        public Company? GetByName(string name)
        {
            if (name != null)
            {
                return _table.SingleOrDefault(c => c.Name == name.ToTitleCase());
            }
            return null;
        }

        public Company? GetByNameOrBusinessCode(string name, string code)
        {
            if (name != null && code != null)
            {
                return _table.SingleOrDefault(c => c.Name == name.ToTitleCase() && c.BusinessCode == code.ToUpper());

            }
            return null;
        }
    }
}
