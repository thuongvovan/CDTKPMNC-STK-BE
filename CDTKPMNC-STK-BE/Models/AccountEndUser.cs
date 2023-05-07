﻿using CDTKPMNC_STK_BE.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace CDTKPMNC_STK_BE.Models
{
    public class AccountEndUser : Account
    {
        public virtual Address Address { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Voucher>? Vouchers { get; set; }
        //public ICollection<ProductItem> ProductItems { get; set; }
    }
}
