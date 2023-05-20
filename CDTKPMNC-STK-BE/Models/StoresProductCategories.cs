//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace CDTKPMNC_STK_BE.Models
//{
//    [PrimaryKey("StoreId", "ProductCategoryId")]
//    public class StoresProductCategories
//    {
//        public Guid? StoreId { get; set; }
//        [ForeignKey("StoreId")]
//        public virtual Store? Store { get; set; } = null!;
//        public Guid? ProductCategoryId { get; set; }
//        [ForeignKey("ProductCategoryId")]
//        public virtual ProductCategory? ProductCategory { get; set; } = null!;
//        public virtual ICollection<ProductItem> ProductItems { get; set; } = null!;
//    }
//}
