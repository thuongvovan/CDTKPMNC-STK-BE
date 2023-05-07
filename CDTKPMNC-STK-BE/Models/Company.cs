using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    

    [Index(nameof(BusinessCode), IsUnique = true)]
    public class Company
    {
        [ForeignKey("AccountPartner")]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string BusinessCode { get; set; } = null!;
        public virtual Address Address { get; set; } = null!;
        [JsonIgnore]
        public virtual AccountPartner AccountPartner { get; set; } = null!;
    }
}
