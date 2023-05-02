using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public record Otp(int OtpValue);
    public class OtpAccount
    {
        [ForeignKey("Account")]
        public Guid Id { get; set; }
        public int? RegisterOtp { get; set; }
        public DateTime? RegisterExpiresOn { get; set; }
        public int? ResetPasswordOtp { get; set; }
        public DateTime? ResetPasswordExpiresOn { get; set; }
        [JsonIgnore]
        public virtual Account? Account { get; set; }
    }
}
