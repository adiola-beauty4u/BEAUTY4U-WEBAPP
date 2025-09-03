using Beauty4u.Interfaces.Dto.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Promotions
{
    public class PromotionDto : IPromotionDto
    {
        public string PromoNo { get; set; } = string.Empty;
        public string PromoType { get; set; } = string.Empty;
        public string PromoTypeName { get; set; } = string.Empty;
        public string PromoDate { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string PromoName { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string IsActive { get; set; } = string.Empty;
        public int SumQty { get; set; }
        public decimal SumAmt { get; set; }
        public decimal SumAdd { get; set; }
        public string StoreList { get; set; } = string.Empty;
        public string StoreCodeList { get; set; } = string.Empty;
        public List<string> StoreCodes { get; set; } = new List<string>();
        public decimal DC { get; set; }
        public string FinalSale { get; set; } = string.Empty;
        public DateTime WriteDate { get; set; }
        public string WriteUser { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; } = string.Empty;
        public List<IPromotionRuleDto> PromotionRules { get; set; } = new List<IPromotionRuleDto>();
        public int PromoRuleCount { get; set; }
    }
}
