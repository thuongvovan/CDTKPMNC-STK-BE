using CDTKPMNC_STK_BE.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public record RegisteredAccount(string Account, string Password, string ConfirmPassword, string Name, Gender Gender, int BirthDate, int BirthMonth, int BirthYear);
    public record LoginedAccount(string Account, string Password);
    public record ChangePasswordAccount(string Account, string OldPassword, string NewPassword, string ConfirmPassword);
    public record ResetPasswordAccount(string Account, string NewPassword, string ConfirmPassword);
    public record VerifyResetPwAccount(string Account, int OTP);

    public enum Gender
    {
        Male = 0,
        Female = 1,
        Others = 2
    }
    public class EndUserAccount
    {
        public Guid Id { get; set; }
        [Required]
        public string? Account { get; set; }
        [Required]
        [JsonIgnore]
        public string? Password { get; set; }
        [JsonIgnore]
        public string? NewPassword { get; set; }
        public bool IsVerified { get; set; }
        [Required]
        public string? Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [JsonIgnore]
        public virtual UserOTP? OTP { get; set; }
        [JsonIgnore]
        public virtual UserToken? Token { get; set; }

    }
}
