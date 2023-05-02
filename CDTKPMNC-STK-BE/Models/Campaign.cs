using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public enum CampaignStatus
    {
        WAITING,
        ENABLED,
        PAUSED,
        REMOVED,
        EXPIRED
    }
    public class Campaign
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "date")]
        public DateOnly StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateOnly EndDate { get; set; }
        public CampaignStatus Status { get; set; }
        public virtual Store Store { get; set; }
        public virtual Game Game { get; set; }
        [JsonIgnore]
        public virtual ICollection<VoucherSeries> VoucherSeries { get; set; }
        [JsonIgnore]
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
