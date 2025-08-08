using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Common.Enums;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto.Promotions;
using Microsoft.Extensions.Logging;

namespace Beauty4u.Business.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly ILogger<PromotionService> _logger;
        public PromotionService(IPromotionRepository promotionRepository, ILogger<PromotionService> logger)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;
        }

        public async Task<ITableData> GetProductPromotionsBySkuAsync(string sku)
        {
            try
            {
                var data = await _promotionRepository.GetProductPromotionsBySkuAsync(sku);

                var tableData = new TableData();
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Promo No",
                    FieldName = nameof(ProductPromotion.PromoNo),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Promo Name",
                    FieldName = nameof(ProductPromotion.PromoName),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Start Date",
                    FieldName = nameof(ProductPromotion.StartDate),
                    DataType = ColumnDataType.Date
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "End Date",
                    FieldName = nameof(ProductPromotion.EndDate),
                    DataType = ColumnDataType.Date
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Promo Type",
                    FieldName = nameof(ProductPromotion.PromoType),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Status",
                    FieldName = nameof(ProductPromotion.Status),
                    DataType = ColumnDataType.Bool
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Is Active",
                    FieldName = nameof(ProductPromotion.IsActive),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Sku",
                    FieldName = nameof(ProductPromotion.Sku),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Cost",
                    FieldName = nameof(ProductPromotion.Cost),
                    DataType = ColumnDataType.Decimal
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Retail Price",
                    FieldName = nameof(ProductPromotion.RetailPrice),
                    DataType = ColumnDataType.Decimal
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "NewPrice",
                    FieldName = nameof(ProductPromotion.NewPrice),
                    DataType = ColumnDataType.Decimal
                });

                foreach (var row in data)
                {
                    var rowData = new RowData()
                    {
                        Cells = new Dictionary<string, ICellData>(),
                    };

                    rowData.Cells.Add(nameof(ProductPromotion.PromoNo), new CellData()
                    {
                        RawValue = row.PromoNo,
                        TextValue = row.PromoNo,
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.PromoName), new CellData()
                    {
                        RawValue = row.PromoName,
                        TextValue = row.PromoName,
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.StartDate), new CellData()
                    {
                        RawValue = row.StartDate,
                        TextValue = row.StartDate.ToShortDateString(),
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.EndDate), new CellData()
                    {
                        RawValue = row.EndDate,
                        TextValue = row.EndDate.ToShortDateString(),
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.PromoType), new CellData()
                    {
                        RawValue = row.PromoType,
                        TextValue = row.PromoType,
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.Status), new CellData()
                    {
                        RawValue = row.Status,
                        TextValue = row.Status.ToString(),
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.IsActive), new CellData()
                    {
                        RawValue = row.IsActive,
                        TextValue = row.IsActive,
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.Sku), new CellData()
                    {
                        RawValue = row.Sku,
                        TextValue = row.Sku,
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.Cost), new CellData()
                    {
                        RawValue = row.Cost,
                        TextValue = $"${row.Cost:F2}",
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.RetailPrice), new CellData()
                    {
                        RawValue = row.RetailPrice,
                        TextValue = $"${row.RetailPrice:F2}",
                    });
                    rowData.Cells.Add(nameof(ProductPromotion.NewPrice), new CellData()
                    {
                        RawValue = row.NewPrice,
                        TextValue = $"${row.NewPrice:F2}",
                    });

                    tableData.Rows.Add(rowData);
                }

                return tableData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new TableData();
            }
        }
    }
}
