namespace Beauty4u.Interfaces.Dto.Products
{
    public interface IBulkProductUpdatePreviewResult
    {
        bool BrandChange { get; set; }
        bool ColorChange { get; set; }
        bool CostChange { get; set; }
        string CurrentBrand { get; set; }
        string CurrentColor { get; set; }
        decimal CurrentCost { get; set; }
        string CurrentItmGroup { get; set; }
        decimal CurrentRetail { get; set; }
        string CurrentSize { get; set; }
        string CurrentStyleCode { get; set; }
        string CurrentStyleDesc { get; set; }
        bool ItmGroupChange { get; set; }
        bool RetailChange { get; set; }
        bool SizeChange { get; set; }
        bool StyleCodeChange { get; set; }
        bool StyleDescChange { get; set; }
        string UPC { get; set; }
        bool UPCExists { get; set; }
        string UpdatedBrand { get; set; }
        string UpdatedColor { get; set; }
        decimal UpdatedCost { get; set; }
        string UpdatedItmGroup { get; set; }
        decimal UpdatedRetail { get; set; }
        string UpdatedSize { get; set; }
        string UpdatedStyleCode { get; set; }
        string UpdatedStyleDesc { get; set; }
        string VendorCode { get; set; }
        string VendorName { get; set; }
        string Sku { get; set; }
        int VendorId { get; set; }
        bool IsNew { get; set; }
        string Result { get; set; }
    }
}