using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.Models
{

    [Owned]
    public class Address
    {
        public virtual AddressProvince? Province { get; set; }
        public virtual AddressDistrict? District { get; set; }
        public virtual AddressWard? Ward { get; set; }
        public string? Street { get; set; }

    }
}
