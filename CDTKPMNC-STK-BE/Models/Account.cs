using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Converters;

namespace CDTKPMNC_STK_BE.Models
{
    public enum AccountType
    {
        Admin,
        Partner,
        EndUser,
    }

    public enum Gender
    {
        Male,
        Female,
        Others
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
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public virtual AccountOtp? Otp { get; set; }
        [JsonIgnore]
        public virtual AccountToken? AccountToken { get; set; }
        [JsonIgnore]
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }
        [InverseProperty("Account")]
        [JsonIgnore]
        public virtual ICollection<Notication> Notications { get; set; } = new List<Notication>();
    }
}
