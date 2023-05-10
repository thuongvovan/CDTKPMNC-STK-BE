using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        [JsonConverter(typeof(StringEnumConverter))]
        public PartnerType PartnerType { get; set; }
        [JsonIgnore]
        public virtual Company? Company { get; set; }
        [JsonIgnore]
        public virtual Store? Store { get; set; }
    }
}
