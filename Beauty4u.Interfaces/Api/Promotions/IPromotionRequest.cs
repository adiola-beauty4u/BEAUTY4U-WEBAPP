namespace Beauty4u.Interfaces.Api.Promotions
{
    public interface IPromotionRequest
    {
        DateTime FromDate { get; set; }
        DateTime PromoDate { get; set; }
        string PromoName { get; set; }
        decimal PromoRate { get; set; }
        string PromoType { get; set; }
        DateTime ToDate { get; set; }
        string CurrentUser { get; set; }
        string PromoNo { get; set; }
        bool PromoStatus { get; set; }
        int SumQty { get; set; }
        decimal SumAmt { get; set; }
        decimal SumAdd { get; set; }
        string StoreCode { get; set; }
    }
}