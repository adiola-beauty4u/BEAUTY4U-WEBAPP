namespace Beauty4u.Models.Api.Promotions
{
    public interface IGetProductPromotionRequest
    {
        bool FromPromoPage { get; set; }
        string PromoNo { get; set; }
    }
}