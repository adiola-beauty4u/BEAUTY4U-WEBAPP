using System.Data;

namespace Beauty4u.Interfaces.Dto.Products
{
    public interface IBulkProductRequestParams
    {
        DataTable BulkProducts { get; set; }
        bool IsScheduled { get; set; }
        string Result { get; set; }
        DateTime StartTime { get; set; }
        int UploadType { get; set; }
        string UserCode { get; set; }
        string VendorCode { get; set; }
        int VendorId { get; set; }
        string VendorName { get; set; }
        DateTime EndTime { get; set; }
        bool IsSuccessful { get; set; }
    }
}