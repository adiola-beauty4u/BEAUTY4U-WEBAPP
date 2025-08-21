using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Models.Dto.Products;

namespace Beauty4u.Models.Api.Auth
{
    public class PromotionProductSearchParams : ProductSearchParams, IPromotionProductSearchParams
    {
        public string PromoType { get; set; } = string.Empty;
        public decimal PromoRate { get; set; }
    }
}
