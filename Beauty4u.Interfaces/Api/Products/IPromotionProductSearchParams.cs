using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Interfaces.Api.Products
{
    public interface IPromotionProductSearchParams : IProductSearchParams
    {
        decimal PromoRate { get; set; }
        string PromoType { get; set; }
    }
}