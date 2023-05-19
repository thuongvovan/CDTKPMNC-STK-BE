using System.ComponentModel.DataAnnotations;

namespace CDTKPMNC_STK_BE.Models
{
    public class Voucher
    {
        public Guid EndUserId { get; set; }
        public virtual AccountEndUser EndUser { get; set; } = null!;
        public Guid VoucherSeriesId { get; set; }
        public virtual VoucherSeries VoucherSeries { get; set; } = null!;
        // public virtual Store Store { get; set; } = null!;
        public Guid? CampaignId { get; set; }
        public virtual Campaign? Campaign { get; set; }
        public DateTime CreatedAt { get; set; }
        [Key]
        public Guid VoucherCode { get; set; } = Guid.NewGuid();
        public bool IsUsed { get; set; }
    }
}
