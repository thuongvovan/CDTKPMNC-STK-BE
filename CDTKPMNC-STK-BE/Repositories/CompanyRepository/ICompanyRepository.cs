using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface ICompanyRepository
    {
        void Add(Company company);
        List<Company> GetAll();
        Company? GetById(Guid id);
        Company? GetByName(string name);
        Company? GetByBusinessCode(string code);
        Company? GetByNameOrBusinessCode(string name, string code);
        void Delete(Company company);
        void DeleteById(Guid id);
        // void UpdateCompany(Company company);

    }
}
