using Beauty4u.Interfaces.Requests;

namespace Beauty4u.Interfaces.Dto.Products
{
    public interface IBulkProductResultDto : IBulkProductDataRequest
    {
        string Sku { get; set; }
        string Result { get; set; }
    }
}