using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public interface IEndUserAccountRepository
    {
        List<EndUserAccount> GetAll();
        EndUserAccount? GetById(Guid id);
        EndUserAccount? GetByAccount(string account);
        void Add(EndUserAccount endUser);
        void Delete(Guid id);
        void Delete(EndUserAccount endUser);
        void Update(EndUserAccount endUser);


    }
}
