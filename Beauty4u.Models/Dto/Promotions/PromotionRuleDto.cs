using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Promotions
{
    public class PromotionRuleDto : IPromotionRuleDto
    {
        public int PromoRuleId { get; set; }
        public string PromoType { get; set; } = string.Empty;
        public decimal PromoRate { get; set; }
        public string VendorCode { get; set; } = string.Empty;
        public string ItemGroup { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string RetailPriceCondition { get; set; } = string.Empty;
        public decimal RetailPriceRate { get; set; }
        public int HqId { get; set; }
    }
}
