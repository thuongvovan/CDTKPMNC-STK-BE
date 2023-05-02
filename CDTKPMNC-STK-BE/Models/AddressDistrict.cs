using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CDTKPMNC_STK_BE.Models;

public class AddressDistrict
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? FullName { get; set; }
    public string? NameEN { get; set; }
    public string? FullNameEN { get; set; }
    public string? ProvinceId { get; set; }
    [JsonIgnore]
    public virtual AddressProvince? Province { get; set; }
    [JsonIgnore]
    public virtual ICollection<AddressWard> Wards { get; set; } = new List<AddressWard>();
}
