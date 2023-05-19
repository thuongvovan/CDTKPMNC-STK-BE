using CDTKPMNC_STK_BE.DataAccess;

namespace CDTKPMNC_STK_BE.BusinessServices.Common
{
    public class CommonService
    {
        public readonly IUnitOfWork _unitOfWork;
        public CommonService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
    }
}
