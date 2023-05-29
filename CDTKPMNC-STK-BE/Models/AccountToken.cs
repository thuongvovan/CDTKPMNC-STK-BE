using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class AccountToken
    {
        [JsonIgnore]
        [ForeignKey("Account")]
        public Guid Id { get; set; }
        [JsonIgnore]
        public virtual Account? Account { get; set; }
        [Required]
        public string? AccessToken { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
