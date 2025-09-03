namespace Beauty4u.Models.Api.Promotions
{
    public class GetProductPromotionRequest : IGetProductPromotionRequest
    {
        public string PromoNo { get; set; } = string.Empty;
        public bool FromPromoPage { get; set; }
    }
}
