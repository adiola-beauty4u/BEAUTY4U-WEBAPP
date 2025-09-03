namespace Beauty4u.Interfaces.Api.Promotions
{
    public interface IPromoTransferRequest
    {
        string PromoNo { get; set; }
        List<string> StoreCodes { get; set; }
    }
}