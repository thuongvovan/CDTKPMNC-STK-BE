using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDTKPMNC_STK_BE.Models
{
    public class Voucher
    {
        public Guid EndUserId { get; set; }
        [ForeignKey("EndUserId")]
        public virtual AccountEndUser EndUser { get; set; } = null!;

        public Guid CampaignId { get; set; }
        public Guid VoucherSeriesId { get; set; }
        [ForeignKey("CampaignId,VoucherSeriesId")]
        public virtual CampaignVoucherSeries CampaignVoucherSeries { get; set; } = null!;
        public Guid CampaignEndUsersId { get; set; }
        [ForeignKey("CampaignEndUsersId")]
        public virtual CampaignEndUsers CampaignEndUsers { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        [Key]
        public Guid VoucherCode { get; set; } = Guid.NewGuid();
        public bool IsUsed { get; set; } = false;
    }
}
