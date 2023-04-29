using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public record RefreshToken(string Token);
    public class UserToken
    {
        [JsonIgnore]
        [ForeignKey("Account")]
        public Guid Id { get; set; }
        [Required]
        public string? AccessToken { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public virtual EndUserAccount? Account { get; set; }
    }
}
