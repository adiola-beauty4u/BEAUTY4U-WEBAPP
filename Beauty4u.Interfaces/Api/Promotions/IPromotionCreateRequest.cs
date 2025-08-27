namespace Beauty4u.Interfaces.Api.Promotions
{
    public interface IPromotionCreateRequest
    {
        DateTime FromDate { get; set; }
        DateTime PromoDate { get; set; }
        string PromoName { get; set; }
        decimal PromoRate { get; set; }
        string PromoType { get; set; }
        DateTime ToDate { get; set; }
        string CurrentUser { get; set; }
    }
}