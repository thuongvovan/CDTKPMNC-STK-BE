using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class Store
    {
        [ForeignKey("AccountPartner")]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Address Address { get; set; } = null!;
        [Column(TypeName = "time(0)")]
        public TimeOnly OpenTime { get; set; }
        [Column(TypeName = "time(0)")]
        public TimeOnly CloseTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsEnable { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductCategory>? ProductCategoies { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductItem>? ProductItems { get; set; }
        [JsonIgnore]
        public virtual AccountPartner AccountPartner { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Campaign>? Campaigns { get; set; }
        [JsonIgnore]
        public virtual ICollection<VoucherSeries>? VoucherSeries { get; set; }
        // [JsonIgnore]
        // public virtual ICollection<Voucher>? Vouchers { get; set; }

    }
}
 