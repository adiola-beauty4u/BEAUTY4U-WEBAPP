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
        public string VendorCode { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string UPC { get; set; } = null!;
        public bool UPCExists { get; set; }
        public string CurrentBrand { get; set; } = null!;
        public string UpdatedBrand { get; set; } = null!;
        public bool BrandChange { get; set; }
        public string CurrentStyleCode { get; set; } = null!;
        public string UpdatedStyleCode { get; set; } = null!;
        public bool StyleCodeChange { get; set; }
        public string CurrentStyleDesc { get; set; } = null!;
        public string UpdatedStyleDesc { get; set; } = null!;
        public bool StyleDescChange { get; set; }
        public string CurrentSize { get; set; } = null!;
        public string UpdatedSize { get; set; } = null!;
        public bool SizeChange { get; set; }
        public string CurrentColor { get; set; } = null!;
        public string UpdatedColor { get; set; } = null!;
        public bool ColorChange { get; set; }
        public decimal CurrentRetail { get; set; }
        public decimal UpdatedRetail { get; set; }
        public bool RetailChange { get; set; }
        public decimal CurrentCost { get; set; }
        public decimal UpdatedCost { get; set; }
        public bool CostChange { get; set; }
        public string CurrentItmGroup { get; set; } = null!;
        public string UpdatedItmGroup { get; set; } = null!;
        public bool ItmGroupChange { get; set; }
        public bool IsNew { get; set; }
        public string Result { get; set; }
    }
}
