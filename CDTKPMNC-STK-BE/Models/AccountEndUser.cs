using CDTKPMNC_STK_BE.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public class AccountEndUser : Account
    {
        public virtual Address? Address { get; set; }
        [JsonIgnore]
        public virtual ICollection<Voucher> Vouchers { get; set; }
        //public ICollection<ProductItem> ProductItems { get; set; }
    }
}
