using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.Models
{

    [Owned]
    public class Address
    {
        [Column("Province")]
        [Required]
        public string? Province { get; set; }
        [Column("District")]
        [Required]
        public string? District { get; set; }
        [Column("Ward")]
        [Required]
        public string? Ward { get; set; }
        [Column("Street")]
        [Required]
        public string? Street { get; set; }

    }
}
