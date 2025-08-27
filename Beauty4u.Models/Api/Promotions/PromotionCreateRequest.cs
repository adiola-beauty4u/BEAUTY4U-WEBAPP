using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Models.Dto.Promotions;

namespace Beauty4u.Models.Api.Promotions
{
    public class PromotionCreateRequest : IPromotionCreateRequest
    {
        public string PromoName { get; set; } = string.Empty;
        public DateTime PromoDate { get; set; }
        public string PromoType { get; set; } = string.Empty;
        public decimal PromoRate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CurrentUser { get; set; } = string.Empty;
        public List<PromotionItems> PromotionItems { get; set; } = new List<PromotionItems>();
        public List<PromotionRules> PromotionRules { get; set; } = new List<PromotionRules>();
    }
}
