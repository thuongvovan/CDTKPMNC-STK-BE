using Castle.Components.DictionaryAdapter;
using CDTKPMNC_STK_BE.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Converters;

namespace CDTKPMNC_STK_BE.Models
{

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
        [JsonIgnore]
        [Required]
        public string? Password { get; set; }
        [JsonIgnore]
        public string? NewPassword { get; set; }
        public string? Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }
        [Column(TypeName = "date")]
        public DateOnly DateOfBirth { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public virtual OtpAccount? Otp { get; set; }
        public virtual TokenAccount? AccountToken { get; set; }
        [JsonIgnore]
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }

    }
}
