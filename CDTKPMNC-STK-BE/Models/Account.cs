using Castle.Components.DictionaryAdapter;
using CDTKPMNC_STK_BE.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public record RegisteredAccount(string UserName, string Password, string Name, Gender Gender, int BirthDate, int BirthMonth, int BirthYear);
    public record LoginedAccount(string UserName, string Password);
    public record ChangePasswordAccount(string UserName, string OldPassword, string NewPassword);
    public record ResetPasswordAccount(string UserName, string NewPassword);
    public record VerifyResetPwAccount(string UserName, int Otp);

    public enum Gender
    {
        Male = 0,
        Female = 1,
        Others = 2
    }
    abstract public class Account
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        [JsonIgnore]
        public string? Password { get; set; }
        [JsonIgnore]
        public string? NewPassword { get; set; }
        public string? Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        [Column(TypeName = "date")]
        public DateOnly DateOfBirth { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public virtual OtpAccount? Otp { get; set; }
        [JsonIgnore]
        public virtual TokenAccount? AccountToken { get; set; }
        [JsonIgnore]
        public bool IsVerified { get; set; }
        [JsonIgnore]
        public DateTime? VerifiedAt { get; set; }
    }
}
