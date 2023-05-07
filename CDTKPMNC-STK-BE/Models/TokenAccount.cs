using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class TokenAccount
    {
        [JsonIgnore]
        [ForeignKey("Account")]
        public Guid Id { get; set; }
        [Required]
        public string? AccessToken { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public virtual Account? Account { get; set; }
    }
}
