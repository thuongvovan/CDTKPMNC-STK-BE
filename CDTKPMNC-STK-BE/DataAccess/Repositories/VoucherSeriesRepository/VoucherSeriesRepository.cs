using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.DataAccess.Repositories.VoucherSeriesRepository
{
    public class VoucherSeriesRepository : CommonRepository<VoucherSeries>, IVoucherSeriesRepository
    {
        public VoucherSeriesRepository(AppDbContext context) : base(context){ }
        
    }
}
