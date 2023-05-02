using System;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Store>? Stores { get; set; } 
        [JsonIgnore]
        public virtual ICollection<ProductItem>? Items { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
    }
}
