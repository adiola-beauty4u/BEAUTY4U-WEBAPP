using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api.Promotions
{
    public class PromotionItems
    {
        public string Sku { get; set; } = string.Empty;
        public decimal RetailPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int? PromoRuleId { get; set; }
    }
}
