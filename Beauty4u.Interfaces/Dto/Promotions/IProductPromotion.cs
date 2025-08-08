namespace Beauty4u.Interfaces.Dto.Promotions
{
    public interface IProductPromotion
    {
        decimal Cost { get; set; }
        DateOnly EndDate { get; set; }
        string IsActive { get; set; }
        decimal NewPrice { get; set; }
        string PromoName { get; set; }
        string PromoNo { get; set; }
        string PromoType { get; set; }
        decimal RetailPrice { get; set; }
        string Sku { get; set; }
        DateOnly StartDate { get; set; }
        bool Status { get; set; }
    }
}