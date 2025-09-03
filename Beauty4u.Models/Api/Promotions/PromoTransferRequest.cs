using Beauty4u.Interfaces.Api.Promotions;

namespace Beauty4u.Models.Api.Promotions
{
    public class PromoTransferRequest : IPromoTransferRequest
    {
        public string PromoNo { get; set; } = string.Empty;
        public List<string> StoreCodes { get; set; } = new List<string>();
    }
}
