using System.ComponentModel.DataAnnotations;

namespace CDTKPMNC_STK_BE.Models
{
    public class Voucher
    {
        public virtual AccountEndUser EndUser { get; set; } = null!;
        public virtual VoucherSeries VoucherSeries { get; set; } = null!;
        // public virtual Store Store { get; set; } = null!;
        public virtual Campaign? Campaign { get; set; }
        public DateTime CreatedAt { get; set; }
        [Key]
        public Guid VoucherCode { get; set; } = Guid.NewGuid();
        public bool IsUsed { get; set; }
    }
}
