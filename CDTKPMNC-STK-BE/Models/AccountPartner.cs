using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public enum PartnerType
    {
        Personal,
        Company
    }


    public class AccountPartner : Account
    {
        public virtual Address Address { get; set; } = null!;
        public PartnerType PertnerType { get; set; }
        [JsonIgnore]
        public virtual Company? Company { get; set; }
        [JsonIgnore]
        public virtual Store? Store { get; set; }
    }
}
