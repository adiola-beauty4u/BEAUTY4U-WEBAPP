using System.Data;

namespace Beauty4u.Interfaces.Dto.Products
{
    public interface ITransferProductParams
    {
        DataTable ProductTransferDetails { get; set; }
        string UserCode { get; set; }
    }
}