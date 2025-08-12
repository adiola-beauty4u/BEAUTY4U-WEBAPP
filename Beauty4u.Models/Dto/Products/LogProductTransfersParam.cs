using Beauty4u.Interfaces.Dto.Products;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class LogProductTransfersParam : ILogProductTransfersParam
    {
        public string UserCode { get; set; } = string.Empty;
        public string StoreCode { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsSuccessful { get; set; }
        public string Result { get; set; } = string.Empty;
        public DataTable IsNew { get; set; } = new DataTable();
        public DataTable CurrentRetailPrice { get; set; } = new DataTable();
        public DataTable NewRetailPrice { get; set; } = new DataTable();
        public DataTable CurrentCost { get; set; } = new DataTable();
        public DataTable NewCost { get; set; } = new DataTable();

    }
}
