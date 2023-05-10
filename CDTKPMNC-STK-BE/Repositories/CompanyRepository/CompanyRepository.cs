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

        public void Add(Company? company)
        {
            if (company != null)
            {
                _dbContext.Companys.Add(company);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Company? company)
        {
            if (company != null)
            {
                _dbContext.Companys.Remove(company);
                _dbContext.SaveChanges();

            }
        }

        public void DeleteById(Guid id)
        {
            var company = _dbContext.Companys.SingleOrDefault(c => c.Id == id);
            Delete(company);
        }

        public List<Company> GetAll()
        {
            return _dbContext.Companys.ToList();
        }

        public Company? GetByBusinessCode(string code)
        {
            if (code != null)
            {
                return _dbContext.Companys.SingleOrDefault(c => c.BusinessCode == code.ToUpper());
            }
            return null;
        }

        public Company? GetById(Guid id)
        {
            return _dbContext.Companys.Find(id);
        }

        public Company? GetByName(string name)
        {
            if (name != null)
            {
                return _dbContext.Companys.SingleOrDefault(c => c.Name == name.ToTitleCase());
            }
            return null;
        }

        public Company? GetByNameOrBusinessCode(string name, string code)
        {
            if (name != null && code != null) 
            {
                return _dbContext.Companys.SingleOrDefault(c => c.Name == name.ToTitleCase() && c.BusinessCode == code.ToUpper());

            }
            return null;
        }
    }
}
