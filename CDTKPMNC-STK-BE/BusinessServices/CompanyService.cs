using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.Models;
using System.Runtime.CompilerServices;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class CompanyService : CommonService
    {
        private readonly ICompanyRepository _companyRepo;
        public CompanyService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
            _companyRepo = _unitOfWork.CompanyRepo;
        }

        public Company? GetById(Guid companyId) 
        { 
            return _companyRepo.GetById(companyId);
        }

        public Company? GetByName(string name)
        {
            if (name != null)
            {
                return _companyRepo.GetByName(name);
            }
            return null;
        }

        public Company? GetByBusinessCode(string businessCode)
        {
            if (businessCode != null)
            {
                return _companyRepo.GetByBusinessCode(businessCode);
            }
            return null;
        }

        public Company? GetByNameOrBusinessCode(string name, string businessCode)
        {
            if (name == null && businessCode == null)
            {
                return null;
            }
            else
            {
                if (name != null && businessCode == null)
                {
                    return _companyRepo.GetByName(name);
                }
                else if (name == null && businessCode != null)
                {
                    return _companyRepo.GetByBusinessCode(businessCode);
                }
                return _companyRepo.GetByNameOrBusinessCode(name!, businessCode!);
            }
        }

        public void Remove(Company? company)
        {
            if (company != null)
            {
                _companyRepo.Delete(company);
            }
        }
    }
}
