
namespace Beauty4u.Models.Api.Products
{
    public interface IProductTransferRequest
    {
        List<string> StoreCodes { get; set; }
        List<string> UPCList { get; set; }
    }
}