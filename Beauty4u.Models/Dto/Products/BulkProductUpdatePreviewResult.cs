using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Models.Dto.Products
{
    public class BulkProductUpdatePreviewResult : IBulkProductUpdatePreviewResult
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string UPC { get; set; } = string.Empty;
        public bool UPCExists { get; set; }
        public string CurrentBrand { get; set; } = string.Empty;
        public string UpdatedBrand { get; set; } = string.Empty;
        public bool BrandChange { get; set; }
        public string CurrentStyleCode { get; set; } = string.Empty;
        public string UpdatedStyleCode { get; set; } = string.Empty;
        public bool StyleCodeChange { get; set; }
        public string CurrentStyleDesc { get; set; } = string.Empty;
        public string UpdatedStyleDesc { get; set; } = string.Empty;
        public bool StyleDescChange { get; set; }
        public string CurrentSize { get; set; } = string.Empty;
        public string UpdatedSize { get; set; } = string.Empty;
        public bool SizeChange { get; set; }
        public string CurrentColor { get; set; } = string.Empty;
        public string UpdatedColor { get; set; } = string.Empty;
        public bool ColorChange { get; set; }
        public decimal CurrentRetail { get; set; }
        public decimal UpdatedRetail { get; set; }
        public bool RetailChange { get; set; }
        public decimal CurrentCost { get; set; }
        public decimal UpdatedCost { get; set; }
        public bool CostChange { get; set; }
        public string CurrentItmGroup { get; set; } = string.Empty;
        public string UpdatedItmGroup { get; set; } = string.Empty;
        public bool ItmGroupChange { get; set; }
        public bool IsNew { get; set; }
        public string Result { get; set; }
    }
}
