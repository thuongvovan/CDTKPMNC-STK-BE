using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace CDTKPMNC_STK_BE.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _dbContext;
        public CompanyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCompany(Company? company)
        {
            if (company != null)
            {
                _dbContext.Companys.Add(company);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteCompany(Company? company)
        {
            if (company != null)
            {
                _dbContext.Companys.Remove(company);
                _dbContext.SaveChanges();

            }
        }

        public void DeleteCompanyById(Guid id)
        {
            var company = _dbContext.Companys.SingleOrDefault(c => c.Id == id);
            DeleteCompany(company);
        }

        public List<Company> GetAllCompanys()
        {
            return _dbContext.Companys.ToList();
        }

        public Company? GetCompanyByBusinessCode(string code)
        {
            if (code != null)
            {
                return _dbContext.Companys.SingleOrDefault(c => c.BusinessCode == code.ToUpper());
            }
            return null;
        }

        public Company? GetCompanyById(Guid id)
        {
            return _dbContext.Companys.Find(id);
        }

        public Company? GetCompanyByName(string name)
        {
            if (name != null)
            {
                return _dbContext.Companys.SingleOrDefault(c => c.Name == name.ToTitleCase());
            }
            return null;
        }

        public Company? GetCompanyByNameOrBusinessCode(string name, string code)
        {
            if (name != null && code != null) 
            {
                return _dbContext.Companys.SingleOrDefault(c => c.Name == name.ToTitleCase() && c.BusinessCode == code.ToUpper());

            }
            return null;
        }
    }
}
