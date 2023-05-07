using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Repositories
{
    public interface ICompanyRepository
    {
        void AddCompany(Company company);
        List<Company> GetAllCompanys();
        Company? GetCompanyById(Guid id);
        Company? GetCompanyByName(string name);
        Company? GetCompanyByBusinessCode(string code);
        Company? GetCompanyByNameOrBusinessCode(string name, string code);
        void DeleteCompany(Company company);
        void DeleteCompanyById(Guid id);
        // void UpdateCompany(Company company);

    }
}
