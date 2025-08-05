using System.Data;

namespace Beauty4u.Interfaces.Dto.Products
{
    public interface ILogProductTransfersParam
    {
        DataTable CurrentCost { get; set; }
        DataTable CurrentRetailPrice { get; set; }
        DateTime EndTime { get; set; }
        DataTable IsNew { get; set; }
        bool IsScheduled { get; set; }
        bool IsSuccessful { get; set; }
        DataTable NewCost { get; set; }
        DataTable NewRetailPrice { get; set; }
        DateTime RequestDate { get; set; }
        string Result { get; set; }
        DateTime StartTime { get; set; }
        string StoreCode { get; set; }
        string UserCode { get; set; }
    }
}