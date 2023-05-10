using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models;
[Table("Add_Wards")]
public class AddressWard
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? FullName { get; set; }
    public string? NameEN { get; set; }
    public string? FullNameEN { get; set; }
    [JsonIgnore]
    public string? DistrictId { get; set; }
    [JsonIgnore]
    public virtual AddressDistrict? District { get; set; }
    [JsonIgnore]
    public string? ProvinceId { get; set; }
    [JsonIgnore]
    public virtual AddressProvince? Province { get; set; }
}
