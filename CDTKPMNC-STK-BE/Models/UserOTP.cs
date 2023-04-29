using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDTKPMNC_STK_BE.Models
{
    public record OTP(int OTPValue);
    public class UserOTP
    {
        [ForeignKey("Account")]
        public Guid Id { get; set; }
        public int? RegisterOTP { get; set; }
        public DateTime? RegisterExpiresOn { get; set; }
        public int? ResetPasswordOTP { get; set; }
        public DateTime? ResetPasswordExpiresOn { get; set; }
        virtual public EndUserAccount? Account { get; set; }
    }
}
