using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class VoucherSeries
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Guid StoreId { get; set; }
        [JsonIgnore]
        public virtual Store Store { get; set; } = null!;
        [JsonIgnore]
        [InverseProperty("VoucherSeries")]
        public virtual ICollection<CampaignVoucherSeries> CampaignVoucherSeriesList { get; set; } = new List<CampaignVoucherSeries>();
    }
}
