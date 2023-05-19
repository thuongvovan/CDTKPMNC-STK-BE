using System.ComponentModel.DataAnnotations;

namespace CDTKPMNC_STK_BE.Models
{

    public class AccountAdmin : Account
    {
        public virtual Address Address { get; set; } = null!;
        public string? Position { get; set; }
        public string? Department { get; set; }
    }
}
