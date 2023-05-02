using System;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models
{
	public class AddressProvince
	{
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? NameEN { get; set; }
        public string? FullNameEN { get; set; }
        [JsonIgnore]
        public virtual ICollection<AddressDistrict> Districts { get; set; } = new List<AddressDistrict>();
        [JsonIgnore]
        public virtual ICollection<AddressWard> Wards { get; set; } = new List<AddressWard>();
    }
}

