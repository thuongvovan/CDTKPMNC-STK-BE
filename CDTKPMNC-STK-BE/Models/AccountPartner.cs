using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
    public enum PertnerType
    {
        Personal,
        Company
    }
    public class AccountPartner : Account
    {
        public virtual Address? Address { get; set; }
        public PertnerType PertnerType { get; set; }
        [JsonIgnore]
        public virtual Company? Company { get; set; }
        [JsonIgnore]
        public virtual Store? Store { get; set; }
    }
}
