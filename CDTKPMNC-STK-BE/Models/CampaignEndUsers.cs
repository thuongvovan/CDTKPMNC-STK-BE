using System.ComponentModel.DataAnnotations.Schema;

namespace CDTKPMNC_STK_BE.Models
{
    public class CampaignEndUsers
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; } = null!;
        public Guid EndUserId { get; set; }
        [ForeignKey("EndUserId")]
        public virtual AccountEndUser EndUser { get; set; } = null!;
        public Guid GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game Game { get; set; } = null!;
        public DateTime PlatAt { get; set; } = DateTime.Now;
        public bool? IsWinner { get; set; }
        //public Guid? VoucherId { get; set; }
        //[ForeignKey("VoucherId")]
        public virtual Voucher? Voucher { get; set; } = null!;
    }
}
