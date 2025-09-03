using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Models.Dto.Promotions;

namespace Beauty4u.Models.Api.Promotions
{
    public class PromotionRequest : IPromotionRequest
    {
        public string StoreCode { get; set; } = string.Empty;
        public string PromoNo { get; set; } = string.Empty;
        public string PromoName { get; set; } = string.Empty;
        public DateTime PromoDate { get; set; }
        public string PromoType { get; set; } = string.Empty;
        public int SumQty { get; set; }
        public decimal SumAmt { get; set; }
        public decimal SumAdd { get; set; }
        public decimal PromoRate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CurrentUser { get; set; } = string.Empty;
        public bool PromoStatus { get; set; }
        public List<PromotionItems> PromotionItems { get; set; } = new List<PromotionItems>();
        public List<PromotionRuleDto> PromotionRules { get; set; } = new List<PromotionRuleDto>();
    }
}
