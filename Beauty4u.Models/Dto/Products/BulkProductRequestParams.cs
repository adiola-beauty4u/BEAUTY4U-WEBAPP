using Beauty4u.Common.Enums;
using Beauty4u.Interfaces.Dto.Products;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class BulkProductRequestParams : IBulkProductRequestParams
    {
        public int VendorId { get; set; }
        public string UserCode { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsScheduled { get; set; }
        public int UploadType { get; set; }
        public bool IsSuccessful { get; set; }
        public DataTable BulkProducts { get; set; } = new DataTable();
        public string Result { get; set; } = string.Empty;
    }
}
