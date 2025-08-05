using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto.Products;

namespace Beauty4u.Models.Api.Products
{
    public class ProductTransferResult : IProductTransferResult
    {
        public string StoreCode { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ITableData TableData { get; set; } = new TableData();
        public List<IBulkProductUpdatePreviewResult> TransferResult { get; set; } = new List<IBulkProductUpdatePreviewResult>();
    }
}
