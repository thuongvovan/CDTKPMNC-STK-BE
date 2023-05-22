using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDTKPMNC_STK_BE.Models
{
    public class ProductItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid StoreId { get; set; }
        [JsonIgnore]
        public virtual Store Store { get; set; } = null!;
        public Guid ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; } = null!;
        //public Guid StoreId { get; set; }
        //public Guid ProductCategoryId { get; set; }
        //[ForeignKey("StoreId,ProductCategoryId")]
        //public virtual StoresProductCategories StoresProductCategories { get; set; } = null!;
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEnable { get; set; }
        public string? ImageUrl { get; set; }
        //public icollection<accountenduser> endusers { get; set; }

    }
}
