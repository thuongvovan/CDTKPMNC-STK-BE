using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public class VoucherSeries
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
        public virtual Campaign? Campaign { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        [Column(TypeName = "date")]
        public DateOnly ExpiresOn { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<Voucher>? Vouchers { get; set; }
    }
}
