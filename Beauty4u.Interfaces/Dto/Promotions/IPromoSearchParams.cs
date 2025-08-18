namespace Beauty4u.Interfaces.Dto.Promotions
{
    public interface IPromoSearchParams
    {
        DateTime? FromDate { get; set; }
        string PromoDescription { get; set; }
        string PromoType { get; set; }
        string Status { get; set; }
        string StoreCode { get; set; }
        DateTime? ToDate { get; set; }
        string PromoNo { get; set; }
    }
}