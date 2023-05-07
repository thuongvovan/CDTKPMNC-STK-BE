﻿using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Instruction { get; set; } = null!;
        public bool IsEnable { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<Campaign> Campaigns { get; set; } = null!;
    }
}
