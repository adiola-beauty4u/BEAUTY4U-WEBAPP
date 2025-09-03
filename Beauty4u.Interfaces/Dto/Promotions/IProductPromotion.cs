namespace Beauty4u.Interfaces.Dto.Promotions
{
    public interface IProductPromotion
    {
        decimal Cost { get; set; }
        DateTime EndDate { get; set; }
        string IsActive { get; set; }
        decimal NewPrice { get; set; }
        string PromoName { get; set; }
        string PromoNo { get; set; }
        string PromoType { get; set; }
        decimal RetailPrice { get; set; }
        string Sku { get; set; }
        DateTime StartDate { get; set; }
        bool Status { get; set; }
        decimal CurrentRetailPrice { get; set; }
        int? PromoRuleId { get; set; }
        string? RulePromoType { get; set; }
        decimal? RulePromoRate { get; set; }
        string? RuleVendorName { get; set; }
        string? RuleCategory { get; set; }
        string? RuleBrand { get; set; }
        string? RuleRetailPrice { get; set; }
    }
}