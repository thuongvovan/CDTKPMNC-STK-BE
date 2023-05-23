using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDTKPMNC_STK_BE.Models
{
    [PrimaryKey("CampaignId", "VoucherSeriesId")]
    public class CampaignVoucherSeries
    {
        public Guid CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; } = null!;
        
        public Guid VoucherSeriesId { get; set; }
        [ForeignKey("VoucherSeriesId")]
        public virtual VoucherSeries VoucherSeries { get; set; } = null!;

        public int Quantity { get; set; }
        [Column(TypeName = "date")]
        public DateOnly ExpiresOn { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();

    }
}
