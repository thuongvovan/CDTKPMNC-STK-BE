using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public class Company
    {
        [ForeignKey("AccountPartner")]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string BusinessCode { get; set; } = null!;
        public virtual Address? Address { get; set; }
        [JsonIgnore]
        public virtual AccountPartner AccountPartner { get; set; } = null!;
    }
}
