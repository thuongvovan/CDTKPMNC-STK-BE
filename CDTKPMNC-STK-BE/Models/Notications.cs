using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDTKPMNC_STK_BE.Models
{
    public class Notication
    {
        public Guid Id { get; set; }
        [ForeignKey("Account")]
        public Guid AccountId { get; set; }
        [JsonIgnore]
        public virtual Account Account { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
