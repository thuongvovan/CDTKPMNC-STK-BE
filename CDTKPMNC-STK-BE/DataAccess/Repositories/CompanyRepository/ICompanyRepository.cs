using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories
{
    public interface ICompanyRepository : ICommonRepository<Company>
    {
        Company? GetByName(string name);
        Company? GetByBusinessCode(string code);
        Company? GetByNameOrBusinessCode(string name, string code);
    }
}
