namespace Beauty4u.Models.Dto.Promotions
{
    public interface IPromotionRuleDto
    {
        string Brand { get; set; }
        int HqId { get; set; }
        string ItemGroup { get; set; }
        decimal PromoRate { get; set; }
        int PromoRuleId { get; set; }
        string PromoType { get; set; }
        string RetailPriceCondition { get; set; }
        decimal RetailPriceRate { get; set; }
        string VendorCode { get; set; }
    }
}