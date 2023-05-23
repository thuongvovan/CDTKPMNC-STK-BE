using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
        public DateTime CreatedAt { get; set; }
        public bool IsEnable { get; set; }
    }
}
