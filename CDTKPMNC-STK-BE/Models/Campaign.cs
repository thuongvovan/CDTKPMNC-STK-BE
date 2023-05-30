using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.Models
{
    public enum CampaignStatus
    {
        UNKNOWN,
        WAITING, // Enable + trước thời gian
        RUNNING, // Enable + trong thời gian
        PENDING, // Disable + trước và trong thời gian
        FINISHED, // trong thời gian + hết voucher
        EXPIRED  // Sau thời gian
    }

    public enum GameRule
    {
        Unlimited,
        UntilWin,
        Limit
    }

    public class Campaign
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateOnly StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateOnly EndDate { get; set; }
        public Guid StoreId { get; set; }
        [JsonIgnore]
        public virtual Store Store { get; set; } = null!;
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; } = null!;
        public int WinRate { get; set; }
        [InverseProperty("Campaign")]
        public virtual ICollection<CampaignVoucherSeries> CampaignVoucherSeriesList { get; set; } = new List<CampaignVoucherSeries>();
        public DateTime CreatedAt { get; set; }
        public bool IsEnable { get; set; }
        [InverseProperty("Campaign")]
        [JsonIgnore]
        public virtual ICollection<CampaignEndUsers> CampaignEndUsersList { get; set; } = new List<CampaignEndUsers>();
        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public CampaignStatus Status { get; set; }
        [JsonIgnore]
        public GameRule GameRule { get; set; } = GameRule.UntilWin;
        public int? NumberOfLimit { get; set; }
    }
}
