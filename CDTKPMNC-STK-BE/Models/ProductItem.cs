using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class ProductItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual ProductCategory Category { get; set; } = null!;
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEnable { get; set; }
        [JsonIgnore]    
        public virtual Store Store { get; set; } = null!;
        //public ICollection<AccountEndUser> EndUsers { get; set; }
    }
}
