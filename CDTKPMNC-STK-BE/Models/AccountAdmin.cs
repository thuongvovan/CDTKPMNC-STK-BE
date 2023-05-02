using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CDTKPMNC_STK_BE.Models
{
    public class AccountAdmin : Account
    {
        public string? Position { get; set; }
        public string? Department { get; set; }
    }
}
