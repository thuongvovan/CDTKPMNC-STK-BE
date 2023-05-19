using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public record Otp(int? OtpValue);
    public class AccountOtp
    {
        [ForeignKey("Account")]
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        public int? RegisterOtp { get; set; }
        [JsonIgnore]
        public DateTime? RegisterExpiresOn { get; set; }
        [JsonIgnore]
        public int? ResetPasswordOtp { get; set; }
        [JsonIgnore]
        public DateTime? ResetPasswordExpiresOn { get; set; }
        [JsonIgnore]
        public virtual Account? Account { get; set; }
    }
}
