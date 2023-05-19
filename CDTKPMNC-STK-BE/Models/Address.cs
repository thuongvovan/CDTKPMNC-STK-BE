using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.Models
{
    [Owned]
    public class Address
    {
        [JsonIgnore]
        public string? WardId { get; set; }
        public virtual AddressWard? Ward { get; set; }
        public string? Street { get; set; }

    }
}
 