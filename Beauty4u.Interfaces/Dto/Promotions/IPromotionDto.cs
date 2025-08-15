namespace Beauty4u.Interfaces.Dto.Promotions
{
    public interface IPromotionDto
    {
        decimal DC { get; set; }
        string PromoName { get; set; }
        string FinalSale { get; set; }
        string StartDate { get; set; }
        DateTime LastUpdate { get; set; }
        string LastUser { get; set; }
        string PromoDate { get; set; }
        string PromoNo { get; set; }
        string PromoType { get; set; }
        string PromoTypeName { get; set; }
        bool Status { get; set; }
        string StoreList { get; set; }
        string EndDate { get; set; }
        DateTime WriteDate { get; set; }
        string WriteUser { get; set; }
        string IsActive { get; set; }
    }
}