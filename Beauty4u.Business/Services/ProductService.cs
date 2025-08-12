using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using Beauty4u.Common.Enums;
using Beauty4u.Common.Helpers;
using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.DataAccess.Api;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Common;
using Beauty4u.Models.DataAccess.B4u;
using Beauty4u.Models.Dto.Products;
using Beauty4u.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Beauty4u.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStoreService _storeService;
        private readonly IFileReadHelper _fileReadHelper;
        private readonly IProductsApi _productsApi;
        private readonly ISystemSetupService _systemSetupService;
        private readonly IDataValidationService _dataValidationService;
        private readonly IItemGroupService _itemGroupService;

        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository productRepository, IFileReadHelper fileReadHelper,
            IDataValidationService dataValidationService, IStoreService storeService, IProductsApi productsApi,
            ICurrentUserService currentUserService, IMapper mapper, ISystemSetupService systemSetupService, IItemGroupService itemGroupService, ILogger<ProductService> logger)
        {
            _productRepository = productRepository; // default to v1 for now
            _fileReadHelper = fileReadHelper;
            _dataValidationService = dataValidationService;
            _storeService = storeService;
            _productsApi = productsApi;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _systemSetupService = systemSetupService;
            _itemGroupService = itemGroupService;
            _logger = logger;
        }

        public async Task<ITableData> BulkProductRegisterAsync(IBulkProduct bulkProductRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var bulkProdRequest = bulkProductRequest as BulkProductRequest;
            var bulkProductRequestParams = new BulkProductRequestParams();
            DataTable bulkProduct = new DataTable();
            if (bulkProdRequest != null)
            {
                DateTime requestStart = DateTime.Now;

                if (bulkProdRequest.ProductFile == null || bulkProdRequest.ProductFile.Length == 0)
                    throw new FileNotFoundException("File is empty or missing.");
                try
                {
                    using var stream = bulkProdRequest.ProductFile.OpenReadStream();
                    //var bulkProduct = await _fileReadHelper.ReadCsvAsync<ProductDto>(stream);
                    bulkProduct = await _fileReadHelper.ReadCsvToDataTableAsync(stream);
                    foreach (var row in bulkProduct.AsEnumerable().ToList())
                    {
                        bool allEmpty = row.ItemArray.All(field =>
                            field == null || string.IsNullOrWhiteSpace(field.ToString()));

                        if (allEmpty)
                        {
                            bulkProduct.Rows.Remove(row);
                        }
                    }

                    var bulkProductResults = await _productRepository.BulkProductRegisterAsync(new BulkProductParams()
                    {
                        UserCode = bulkProductRequest.UserCode,
                        VendorCode = bulkProductRequest.VendorCode,
                        VendorId = bulkProductRequest.VendorId,
                        VendorName = bulkProductRequest.VendorName,
                        BulkProducts = DataTableHelper.ToProductDataTable(bulkProduct)
                    });

                    //var bulkProductResults = DataTableHelper.DataTableToList<BulkProductResultDto>(dtResult);
                    TableData tableOutput = new TableData();

                    TableData tableData = new TableData();
                    tableData.Columns.Add(new ColumnData() { FieldName = "Brand", Header = "Brand" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "StyleCode", Header = "StyleCode" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "StyleDesc", Header = "Product Description" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Size", Header = "Size" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Color", Header = "Color" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Retail", Header = "Retail Price", DataType = ColumnDataType.Money });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Cost", Header = "Cost", DataType = ColumnDataType.Money });
                    tableData.Columns.Add(new ColumnData() { FieldName = "ItmGroup", Header = "Category" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Sku", Header = "Sku" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Result", Header = "Result" });

                    foreach (BulkProductResultDto bulkProductResult in bulkProductResults)
                    {
                        var cellDataDict = new Dictionary<string, ICellData>
                        {
                            ["Brand"] = new CellData { RawValue = bulkProductResult.Brand, TextValue = bulkProductResult.Brand },
                            ["StyleCode"] = new CellData { RawValue = bulkProductResult.StyleCode, TextValue = bulkProductResult.StyleCode },
                            ["StyleDesc"] = new CellData { RawValue = bulkProductResult.StyleDesc, TextValue = bulkProductResult.StyleDesc },
                            ["Size"] = new CellData { RawValue = bulkProductResult.Size, TextValue = bulkProductResult.Size },
                            ["Color"] = new CellData { RawValue = bulkProductResult.Color, TextValue = bulkProductResult.Color },
                            ["Retail"] = new CellData { RawValue = bulkProductResult.Retail, TextValue = $"${bulkProductResult.Retail:F2}" },
                            ["Cost"] = new CellData { RawValue = bulkProductResult.Cost, TextValue = $"${bulkProductResult.Cost:F2}" },
                            ["ItmGroup"] = new CellData { RawValue = bulkProductResult.ItmGroup, TextValue = bulkProductResult.ItmGroup },
                            ["UPC"] = new CellData { RawValue = bulkProductResult.UPC, TextValue = bulkProductResult.UPC },
                            ["Sku"] = new CellData { RawValue = bulkProductResult.Sku, TextValue = bulkProductResult.Sku },
                            ["Result"] = new CellData { RawValue = bulkProductResult.Result, TextValue = bulkProductResult.Result }
                        };

                        tableData.Rows.Add(new RowData()
                        {
                            Cells = cellDataDict,
                            RowKey = bulkProductResult.UPC,
                            IsValid = bulkProductResult.Result == "Saved",
                            CssClass = bulkProductResult.Result == "Saved" ? "row-valid" : "row-invalid"
                        });
                    }
                    tableOutput.TableGroups.Add(tableData);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Bulk Product Registration by {currentUser?.Claims["UserCode"]!}");
                    sb.AppendLine($"Bulk Product Registration Start: {requestStart}");
                    sb.AppendLine($"Bulk Product Registration End: {DateTime.Now}");
                    sb.AppendLine($"Bulk Product Registration Count: {bulkProductResults.Count}");
                    sb.AppendLine($"Bulk Product Filename: {bulkProdRequest.ProductFile.FileName}");
                    _logger.LogInformation(sb.ToString());

                    bulkProductRequestParams = new BulkProductRequestParams()
                    {
                        UserCode = bulkProductRequest.UserCode,
                        VendorCode = bulkProductRequest.VendorCode,
                        VendorId = bulkProductRequest.VendorId,
                        VendorName = bulkProductRequest.VendorName,
                        BulkProducts = DataTableHelper.ToProductDataTable(bulkProduct),
                        IsScheduled = false,
                        StartTime = requestStart,
                        EndTime = DateTime.Now,
                        UploadType = (int)BulkProductUploadType.Register,
                        IsSuccessful = true,
                        Result = "Saved successfully."
                    };
                    await _productRepository.LogBulkProductRequestAsync(bulkProductRequestParams);
                    return tableOutput;
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Bulk Product Registration by {currentUser?.Claims["UserCode"]!}");
                    sb.AppendLine($"Bulk Product Registration Start: {requestStart}");
                    sb.AppendLine($"Bulk Product Registration End: {DateTime.Now}");
                    sb.AppendLine($"Bulk Product Filename: {bulkProdRequest.ProductFile.FileName}");
                    sb.AppendLine($"Bulk Product Error: {ex.Message}");
                    _logger.LogError(sb.ToString());

                    bulkProductRequestParams = new BulkProductRequestParams()
                    {
                        UserCode = bulkProductRequest.UserCode,
                        VendorCode = bulkProductRequest.VendorCode,
                        VendorId = bulkProductRequest.VendorId,
                        VendorName = bulkProductRequest.VendorName,
                        BulkProducts = DataTableHelper.ToProductDataTable(bulkProduct),
                        IsScheduled = false,
                        StartTime = requestStart,
                        EndTime = DateTime.Now,
                        UploadType = (int)BulkProductUploadType.Register,
                        IsSuccessful = false,
                        Result = ex.Message
                    };
                    await _productRepository.LogBulkProductRequestAsync(bulkProductRequestParams);
                }
            }

            return new TableData();
        }

        public async Task<ITableData> BulkProductUpdateAsync(IFormFile productFile)
        {

            if (productFile == null || productFile.Length == 0)
                throw new FileNotFoundException("File is empty or missing.");
            var currentUser = _currentUserService.GetCurrentUser();
            TableData tableData = new TableData();
            DateTime requestStart = DateTime.Now;
            StringBuilder sb = new StringBuilder();

            using var stream = productFile.OpenReadStream();
            var initialBulkProduct = await _fileReadHelper.ReadCsvToDataTableAsync(stream);

            foreach (var row in initialBulkProduct.AsEnumerable().ToList())
            {
                bool allEmpty = row.ItemArray.All(field =>
                    field == null || string.IsNullOrWhiteSpace(field.ToString()));

                if (allEmpty)
                {
                    initialBulkProduct.Rows.Remove(row);
                }
            }

            var bulkProduct = DataTableHelper.ToProductDataTable(initialBulkProduct);

            //var bulkProduct = await _fileReadHelper.ReadCsvToDataTableAsync(stream);
            var bulkProductList = DataTableHelper.DataTableToList<BulkProductDataRequest>(bulkProduct);
            var bulkProductUpdatePreviewResults = await this._productRepository.BulkUpdatePreview(bulkProduct);
            var updateBulkProductDict = bulkProductUpdatePreviewResults
                .GroupBy(x => x.VendorCode).ToDictionary(x => x.Key, x => x.ToList());

            tableData.TableGroups = new List<ITableData>();
            List<IColumnData> columns = new List<IColumnData>();
            columns.Add(new ColumnData() { FieldName = "Sku", Header = "Sku" });
            columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC" });
            columns.Add(new ColumnData() { FieldName = "PreviousRetail", Header = "Previous Retail Price" });
            columns.Add(new ColumnData() { FieldName = "UpdatedRetail", Header = "Updated Retail Price" });
            columns.Add(new ColumnData() { FieldName = "PreviousCost", Header = "Previous Cost" });
            columns.Add(new ColumnData() { FieldName = "UpdatedCost", Header = "Updated Cost" });

            foreach (var item in updateBulkProductDict)
            {
                var updatedItems = item.Value.Where(x => x.RetailChange || x.CostChange).ToList();
                var upcList = updatedItems.Select(x => x.UPC).ToList();
                var bulkProductParams = new BulkProductParams()
                {
                    UserCode = currentUser?.Claims["UserCode"]!,
                    VendorCode = item.Key,
                    VendorId = item.Value.First().VendorId,
                    VendorName = item.Value.First().VendorName,
                    BulkProducts = DataTableHelper.ToProductDataTable(bulkProductList.Where(x => upcList.Contains(x.UPC)).ToList()),
                };
                DateTime productVendorStart = DateTime.Now;
                var bulkProductRequestParams = new BulkProductRequestParams();
                try
                {
                    sb.Clear();
                    sb.AppendLine($"Bulk Product Update by {currentUser?.Claims["UserCode"]!}");
                    sb.AppendLine($"Bulk Product Update Vendor: {item.Value.First().VendorName} ({item.Key})");
                    sb.AppendLine($"Bulk Product Update Start: {requestStart}");
                    sb.AppendLine($"Bulk Product Update Filename: {productFile.FileName}");
                    // Might change in the future. Get updated cost and retail price only
                    await _productRepository.BulkProductUpdateAsync(bulkProductParams);

                    var tableGroup = new TableData();
                    tableGroup.TableName = item.Key;
                    tableGroup.Columns = columns.ToList();
                    foreach (var previewResult in updatedItems)
                    {
                        var rowData = new RowData();
                        rowData.Cells.Add(nameof(previewResult.Sku), new CellData()
                        {
                            RawValue = previewResult.Sku,
                            TextValue = previewResult.Sku,
                        });
                        rowData.Cells.Add(nameof(previewResult.UPC), new CellData()
                        {
                            RawValue = previewResult.UPC,
                            TextValue = previewResult.UPC,
                        });
                        rowData.Cells.Add("PreviousRetail", new CellData()
                        {
                            RawValue = previewResult.CurrentRetail,
                            TextValue = $"${previewResult.CurrentRetail:F2}",
                            CssClass = ""
                        });
                        rowData.Cells.Add(nameof(previewResult.UpdatedRetail), new CellData()
                        {
                            RawValue = previewResult.UpdatedRetail,
                            TextValue = $"${previewResult.UpdatedRetail:F2}",
                            IsValid = true,
                            CssClass = previewResult.CurrentRetail > previewResult.UpdatedRetail ? "cell-changed" : previewResult.CurrentRetail < previewResult.UpdatedRetail ? "cell-valid" : "",
                            Tooltip = previewResult.CurrentRetail > previewResult.UpdatedRetail ? "Retail price decrease\n" : previewResult.CurrentRetail < previewResult.UpdatedRetail ? "Retail price increase\n" : "",
                        });
                        rowData.Cells.Add("PreviousCost", new CellData()
                        {
                            RawValue = previewResult.CurrentCost,
                            TextValue = $"${previewResult.CurrentCost:F2}",
                            CssClass = ""
                        });
                        rowData.Cells.Add(nameof(previewResult.UpdatedCost), new CellData()
                        {
                            RawValue = previewResult.UpdatedCost,
                            TextValue = $"${previewResult.UpdatedCost:F2}",
                            IsValid = true,
                            CssClass = previewResult.CurrentCost > previewResult.UpdatedCost ? "cell-changed" : previewResult.CurrentCost < previewResult.UpdatedCost ? "cell-valid" : "",
                            Tooltip = previewResult.CurrentCost > previewResult.UpdatedCost ? "Cost decrease\n" : previewResult.CurrentCost < previewResult.UpdatedCost ? "Cost increase\n" : "",
                        });
                        tableGroup.Rows.Add(rowData);
                    }
                    tableData.TableGroups.Add(tableGroup);

                    sb.AppendLine($"Bulk Product Update Vendor Count: {item.Key} - {updatedItems.Count}");

                    bulkProductRequestParams = new BulkProductRequestParams()
                    {
                        UserCode = currentUser?.Claims["UserCode"]!,
                        VendorCode = bulkProductParams.VendorCode,
                        VendorId = bulkProductParams.VendorId,
                        VendorName = bulkProductParams.VendorName,
                        BulkProducts = bulkProductParams.BulkProducts,
                        IsScheduled = false,
                        StartTime = productVendorStart,
                        EndTime = DateTime.Now,
                        UploadType = (int)BulkProductUploadType.Update,
                        IsSuccessful = true,
                        Result = "Saved successfully."
                    };

                    sb.AppendLine($"Bulk Product Update Request End: {DateTime.Now}");


                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Bulk Product Update Error: {ex.Message}");
                    sb.AppendLine($"Bulk Product Update Request End: {DateTime.Now}");
                    _logger.LogError(sb.ToString());

                    bulkProductRequestParams = new BulkProductRequestParams()
                    {
                        UserCode = currentUser?.Claims["UserCode"]!,
                        VendorCode = bulkProductParams.VendorCode,
                        VendorId = bulkProductParams.VendorId,
                        VendorName = bulkProductParams.VendorName,
                        BulkProducts = bulkProductParams.BulkProducts,
                        IsScheduled = false,
                        StartTime = productVendorStart,
                        EndTime = DateTime.Now,
                        UploadType = (int)BulkProductUploadType.Update,
                        IsSuccessful = false,
                        Result = ex.Message
                    };
                }
                finally
                {
                    _logger.LogInformation(sb.ToString());
                    await _productRepository.LogBulkProductRequestAsync(bulkProductRequestParams);
                }
            }

            return tableData;
        }

        public async Task<IProductSearchResult> ProductTransferSearchAsync(DateOnly dateStart, DateOnly dateEnd)
        {
            var productResultSearch = new ProductSearchResult();

            var output = await _productRepository.ProductTransferSearchAsync(dateStart, dateEnd);
            productResultSearch.Products.AddRange(output);
            TableData outputTable = new TableData();
            outputTable.TableGroups = new List<ITableData>();
            TableData tableData = new TableData();
            tableData.Columns = new List<IColumnData>();
            tableData.Rows = new List<IRowData>();
            if (output != null)
            {
                tableData.Columns.Add(new ColumnData() { FieldName = "VendorName", Header = "Vendor" });
                tableData.Columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC" });
                tableData.Columns.Add(new ColumnData() { FieldName = "Sku", Header = "Sku" });
                tableData.Columns.Add(new ColumnData() { FieldName = "Brand", Header = "Brand" });
                tableData.Columns.Add(new ColumnData() { FieldName = "StyleCode", Header = "Style Code" });
                tableData.Columns.Add(new ColumnData() { FieldName = "StyleDesc", Header = "Product Description" });
                tableData.Columns.Add(new ColumnData() { FieldName = "Size", Header = "Size" });
                tableData.Columns.Add(new ColumnData() { FieldName = "Color", Header = "Color" });
                tableData.Columns.Add(new ColumnData() { FieldName = "Retail", Header = "Retail Price", DataType = ColumnDataType.Money });
                tableData.Columns.Add(new ColumnData() { FieldName = "Cost", Header = "Cost", DataType = ColumnDataType.Money });
                tableData.Columns.Add(new ColumnData() { FieldName = "ItmGroup", Header = "Category" });

                foreach (var transferItem in output)
                {
                    var rowData = new RowData
                    {
                        Cells = new Dictionary<string, ICellData>(),
                        IsValid = true,
                    };

                    rowData.Cells.Add(nameof(transferItem.VendorName), new CellData()
                    {
                        RawValue = transferItem.VendorName,
                        TextValue = transferItem.VendorName,
                        IsValid = true,
                    });

                    rowData.Cells.Add(nameof(transferItem.Sku), new CellData()
                    {
                        RawValue = transferItem.Sku,
                        TextValue = transferItem.Sku,
                        IsValid = true,
                    });

                    rowData.Cells.Add(nameof(transferItem.UPC), new CellData()
                    {
                        RawValue = transferItem.UPC,
                        TextValue = transferItem.UPC,
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.Brand), new CellData()
                    {
                        RawValue = transferItem.Brand,
                        TextValue = transferItem.Brand,
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.StyleCode), new CellData()
                    {
                        RawValue = transferItem.StyleCode,
                        TextValue = transferItem.StyleCode,
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.StyleDesc), new CellData()
                    {
                        RawValue = transferItem.StyleDesc,
                        TextValue = transferItem.StyleDesc,
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.Size), new CellData()
                    {
                        RawValue = transferItem.Size,
                        TextValue = transferItem.Size,
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.Color), new CellData()
                    {
                        RawValue = transferItem.Color,
                        TextValue = transferItem.Color,
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.RetailPrice), new CellData()
                    {
                        RawValue = transferItem.RetailPrice,
                        TextValue = $"${transferItem.RetailPrice:F2}",
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.Cost), new CellData()
                    {
                        RawValue = transferItem.RetailPrice,
                        TextValue = $"${transferItem.Cost:F2}",
                        IsValid = true,
                    });
                    rowData.Cells.Add(nameof(transferItem.ItmGroup), new CellData()
                    {
                        RawValue = transferItem.ItmGroup,
                        TextValue = transferItem.ItmGroup,
                        IsValid = true,
                    });

                    tableData.Rows.Add(rowData);
                }

                outputTable.TableGroups.Add(tableData);
            }
            productResultSearch.TableData = outputTable;
            return productResultSearch;
        }

        public async Task<IBulkProductPreviewResult> BulkRegisterPreviewAsync(IBulkProduct bulkProductRequest)
        {
            var bulkProductPreviewResult = new BulkProductPreviewResult();
            var bulkProdRequest = bulkProductRequest as BulkProductRequest;
            if (bulkProdRequest != null)
            {
                if (bulkProdRequest.ProductFile == null || bulkProdRequest.ProductFile.Length == 0)
                {
                    return new BulkProductPreviewResult() { IsValid = false, PreviewResult = "File is empty or missing." };
                }

                using var stream = bulkProdRequest.ProductFile.OpenReadStream();
                var bulkProduct = await _fileReadHelper.ReadCsvToDataTableAsync(stream);
                foreach (var row in bulkProduct.AsEnumerable().ToList())
                {
                    bool allEmpty = row.ItemArray.All(field =>
                        field == null || string.IsNullOrWhiteSpace(field.ToString()));

                    if (allEmpty)
                    {
                        bulkProduct.Rows.Remove(row);
                    }
                }
                string[] columnNames = new string[] { "Brand", "Style Code", "Product Description", "Size", "Color", "Retail Price", "Cost", "Category", "UPC" };

                bool hasExactColumns = bulkProduct.Columns.Count == columnNames.Length &&
                                                            columnNames.Select((name, index) => name.Trim().Equals(bulkProduct.Columns[index].ColumnName.Trim(), StringComparison.OrdinalIgnoreCase))
                                                        .All(result => result);

                if (!hasExactColumns)
                {
                    TableData errorOutput = new TableData();
                    TableData invalidfile = new TableData();
                    invalidfile.Columns = bulkProduct.Columns
                                    .Cast<DataColumn>()
                                    .Select(c => c.ColumnName)
                                    .Select(x => new ColumnData()
                                    {
                                        FieldName = x,
                                        Header = x
                                    }).ToList<IColumnData>();
                    errorOutput.TableGroups.Add(invalidfile);
                    return new BulkProductPreviewResult() { IsValid = false, PreviewResult = "CSV does not have the exact required columns in the correct order.", TableData = errorOutput };
                }

                var bulkProductList = DatatableToBulkProductDataRequest(bulkProduct);

                List<ColumnData> columns = new List<ColumnData>();
                columns.Add(new ColumnData() { FieldName = "Brand", Header = "Brand" });
                columns.Add(new ColumnData() { FieldName = "StyleCode", Header = "Style Code" });
                columns.Add(new ColumnData() { FieldName = "StyleDesc", Header = "Product Description" });
                columns.Add(new ColumnData() { FieldName = "Size", Header = "Size" });
                columns.Add(new ColumnData() { FieldName = "Color", Header = "Color" });
                columns.Add(new ColumnData() { FieldName = "Retail", Header = "Retail Price", DataType = ColumnDataType.Money });
                columns.Add(new ColumnData() { FieldName = "Cost", Header = "Cost", DataType = ColumnDataType.Money });
                columns.Add(new ColumnData() { FieldName = "ItmGroup", Header = "Category" });
                columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC", DataType = ColumnDataType.Int });
                TableData tableData = new TableData();

                var initialTableData = new TableData();
                initialTableData.Columns = columns.ToList<IColumnData>();
                initialTableData.TableName = "Bulk Product Register";
                _dataValidationService.ValidateTable(initialTableData, bulkProduct, "UPC");

                if (initialTableData.Rows.Any(x => !x.IsValid))
                {
                    var outputTable = new TableData();
                    outputTable.TableGroups.Add(initialTableData);
                    return new BulkProductPreviewResult() { IsValid = false, PreviewResult = "Invalid rows detected.", TableData = outputTable };
                }


                TableData tableGroup = new TableData();
                tableGroup.TableName = $"{bulkProductRequest.VendorName} ({bulkProductRequest.VendorCode})";

                tableGroup.Columns = columns.ToList<IColumnData>();

                await ValidateBulkProductRegister(tableGroup, bulkProductList);

                tableData.TableGroups.Add(tableGroup);
                return new BulkProductPreviewResult() { IsValid = true, PreviewResult = "", TableData = tableData };
            }
            return bulkProductPreviewResult;
        }

        private async Task ValidateBulkProductRegister(ITableData tableData, List<BulkProductResultDto> bulkProdList)
        {
            var upcList = DataTableHelper.StringListParameter(bulkProdList.Select(x => x.UPC).ToList());
            var upcValidateResult = await this._productRepository.ValidateUPCListAsync(upcList);
            var itemGroups = await this._itemGroupService.GetActiveItemGroupsAsync();
            var upcDict = upcValidateResult.ToDictionary(x => x.UPC);
            foreach (var prodct in bulkProdList)
            {
                var rowData = new RowData
                {
                    Cells = new Dictionary<string, ICellData>(),
                    IsValid = !upcDict[prodct.UPC].ExistsInMIS && !upcDict[prodct.UPC].ExistsInB4u,
                    CssClass = upcDict[prodct.UPC].ExistsInMIS || upcDict[prodct.UPC].ExistsInB4u ? "row-invalid" : "",
                    Tooltip = upcDict[prodct.UPC].ExistsInMIS || upcDict[prodct.UPC].ExistsInB4u ? "Existing product.\n" : "",
                };

                rowData.Cells.Add(nameof(prodct.Brand), new CellData()
                {
                    RawValue = prodct.Brand,
                    TextValue = prodct.Brand,
                    IsValid = !string.IsNullOrWhiteSpace(prodct.Brand) && prodct.Brand.Length <= 50,
                    Tooltip = string.IsNullOrWhiteSpace(prodct.Brand) ? $"{nameof(prodct.Brand)} required!" : prodct.Brand.Length > 50 ? $"{nameof(prodct.Brand)} too long." : ""
                });
                rowData.Cells.Add(nameof(prodct.StyleCode), new CellData()
                {
                    RawValue = prodct.StyleCode,
                    TextValue = prodct.StyleCode,
                    IsValid = string.IsNullOrWhiteSpace(prodct.StyleCode) || (!string.IsNullOrWhiteSpace(prodct.StyleCode) && prodct.StyleCode.Length <= 50),
                    Tooltip = !string.IsNullOrWhiteSpace(prodct.StyleCode) && prodct.StyleCode.Length > 50 ? $"Style Code too long." : ""
                });
                rowData.Cells.Add(nameof(prodct.StyleDesc), new CellData()
                {
                    RawValue = prodct.StyleDesc,
                    TextValue = prodct.StyleDesc,
                    IsValid = !string.IsNullOrWhiteSpace(prodct.StyleDesc) && prodct.StyleDesc.Length <= 200,
                    Tooltip = string.IsNullOrWhiteSpace(prodct.StyleDesc) ? $"Product Description required!" : prodct.StyleDesc.Length > 200 ? $"Product Description too long." : ""
                });
                rowData.Cells.Add(nameof(prodct.Size), new CellData()
                {
                    RawValue = prodct.Size,
                    TextValue = prodct.Size,
                    IsValid = (!string.IsNullOrWhiteSpace(prodct.Size) && prodct.Size.Length <= 50) || string.IsNullOrWhiteSpace(prodct.Size),
                    Tooltip = prodct.Size.Length > 50 ? $"{nameof(prodct.Size)} too long." : ""
                });
                rowData.Cells.Add(nameof(prodct.Color), new CellData()
                {
                    RawValue = prodct.Color,
                    TextValue = prodct.Color,
                    IsValid = (!string.IsNullOrWhiteSpace(prodct.Color) && prodct.Color.Length <= 50) || string.IsNullOrWhiteSpace(prodct.Color),
                    Tooltip = prodct.Color.Length > 50 ? $"{nameof(prodct.Color)} too long." : ""
                });
                rowData.Cells.Add(nameof(prodct.Retail), new CellData()
                {
                    RawValue = prodct.Retail,
                    TextValue = $"${prodct.Retail:F2}",
                });
                rowData.Cells.Add(nameof(prodct.Cost), new CellData()
                {
                    RawValue = prodct.Cost,
                    TextValue = $"${prodct.Cost:F2}",
                });
                rowData.Cells.Add(nameof(prodct.ItmGroup), new CellData()
                {
                    RawValue = itemGroups.ContainsKey(prodct.ItmGroup) ? $"{itemGroups[prodct.ItmGroup].Code} ({itemGroups[prodct.ItmGroup].Name})" : prodct.ItmGroup,
                    TextValue = itemGroups.ContainsKey(prodct.ItmGroup) ? $"{itemGroups[prodct.ItmGroup].Code} ({itemGroups[prodct.ItmGroup].Name})" : prodct.ItmGroup,
                    IsValid = itemGroups.ContainsKey(prodct.ItmGroup),
                    Tooltip = !itemGroups.ContainsKey(prodct.ItmGroup) ? $"Invalid category." : ""
                });
                rowData.Cells.Add(nameof(prodct.UPC), new CellData()
                {
                    RawValue = prodct.UPC,
                    TextValue = prodct.UPC,
                    IsValid = !string.IsNullOrWhiteSpace(prodct.UPC) && prodct.UPC.Length >= 12,
                    Tooltip = string.IsNullOrWhiteSpace(prodct.UPC) ? $"{nameof(prodct.UPC)} required!" : prodct.UPC.Length < 12 ? $"{nameof(prodct.UPC)} too short." : ""
                });

                if (rowData.Cells.Values.Any(cell => !cell.IsValid))
                {
                    // Handle invalid row
                    rowData.IsValid = false;
                    rowData.CssClass = "row-invalid";
                    rowData.Tooltip += string.Join(".", rowData.Cells
                                                .Where(kv => !kv.Value.IsValid && !string.IsNullOrWhiteSpace(kv.Value.Tooltip))
                                                .Select(kv => kv.Value.Tooltip)
                                                .ToList());
                }
                tableData.Rows.Add(rowData);
            }
        }

        public async Task<IBulkProductPreviewResult> BulkUpdatePreviewAsync(IFormFile productFile)
        {
            if (productFile == null || productFile.Length == 0)
            {
                return new BulkProductPreviewResult() { IsValid = false, PreviewResult = "File is empty or missing." };
            }

            using var stream = productFile.OpenReadStream();
            var bulkProduct = await _fileReadHelper.ReadCsvToDataTableAsync(stream);
            foreach (var row in bulkProduct.AsEnumerable().ToList())
            {
                bool allEmpty = row.ItemArray.All(field =>
                    field == null || string.IsNullOrWhiteSpace(field.ToString()));

                if (allEmpty)
                {
                    bulkProduct.Rows.Remove(row);
                }
            }

            string[] columnNames = new string[] { "Brand", "Style Code", "Product Description", "Size", "Color", "Retail Price", "Cost", "Category", "UPC" };

            bool hasExactColumns = bulkProduct.Columns.Count == columnNames.Length &&
                                                        columnNames.Select((name, index) => name.Trim().Equals(bulkProduct.Columns[index].ColumnName.Trim(), StringComparison.OrdinalIgnoreCase))
                                                    .All(result => result);

            if (!hasExactColumns)
            {
                TableData errorOutput = new TableData();
                TableData invalidfile = new TableData();
                invalidfile.Columns = bulkProduct.Columns
                                .Cast<DataColumn>()
                                .Select(c => c.ColumnName.Trim().ToLower())
                                .Select(x => new ColumnData()
                                {
                                    FieldName = x,
                                    Header = x
                                }).ToList<IColumnData>();
                errorOutput.TableGroups.Add(invalidfile);
                return new BulkProductPreviewResult() { IsValid = false, PreviewResult = "CSV does not have the exact required columns in the correct order.", TableData = errorOutput };
            }

            var bulkProductList = DataTableHelper.DataTableToList<BulkProductResultDto>(bulkProduct);
            List<IColumnData> columns = new List<IColumnData>();
            columns.Add(new ColumnData() { FieldName = "Sku", Header = "Sku" });
            columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC" });
            columns.Add(new ColumnData() { FieldName = "CurrentBrand", Header = "Current Brand" });
            columns.Add(new ColumnData() { FieldName = "UpdatedBrand", Header = "Updated Brand" });
            columns.Add(new ColumnData() { FieldName = "CurrentStyleCode", Header = "Current StyleCode" });
            columns.Add(new ColumnData() { FieldName = "UpdatedStyleCode", Header = "Updated StyleCode" });
            columns.Add(new ColumnData() { FieldName = "CurrentStyleDesc", Header = "Current ProductDescription" });
            columns.Add(new ColumnData() { FieldName = "UpdatedStyleDesc", Header = "Updated ProductDescription" });
            columns.Add(new ColumnData() { FieldName = "CurrentSize", Header = "Current Size" });
            columns.Add(new ColumnData() { FieldName = "UpdatedSize", Header = "Updated Size" });
            columns.Add(new ColumnData() { FieldName = "CurrentColor", Header = "Current Color" });
            columns.Add(new ColumnData() { FieldName = "UpdatedColor", Header = "Updated Color" });
            columns.Add(new ColumnData() { FieldName = "CurrentRetail", Header = "Current Retail" });
            columns.Add(new ColumnData() { FieldName = "UpdatedRetail", Header = "Updated Retail" });
            columns.Add(new ColumnData() { FieldName = "CurrentCost", Header = "Current Cost" });
            columns.Add(new ColumnData() { FieldName = "UpdatedCost", Header = "Updated Cost" });
            columns.Add(new ColumnData() { FieldName = "CurrentItmGroup", Header = "Current Category" });
            columns.Add(new ColumnData() { FieldName = "UpdatedItmGroup", Header = "Updated Category" });

            var initialTableData = new TableData();
            initialTableData.Columns.Add(new ColumnData() { FieldName = "Brand", Header = "Brand" });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "StyleCode", Header = "Style Code" });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "StyleDesc", Header = "Product Description" });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "Size", Header = "Size" });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "Color", Header = "Color" });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "Retail", Header = "Retail Price", DataType = ColumnDataType.Money });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "Cost", Header = "Cost", DataType = ColumnDataType.Money });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "ItmGroup", Header = "Category" });
            initialTableData.Columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC" });
            initialTableData.TableName = "Bulk Product Update File";
            _dataValidationService.ValidateTable(initialTableData, bulkProduct, "UPC");

            if (initialTableData.Rows.Any(x => !x.IsValid))
            {
                var outputTable = new TableData();
                outputTable.TableGroups.Add(initialTableData);
                return new BulkProductPreviewResult() { IsValid = false, PreviewResult = "Invalid rows detected.", TableData = outputTable };
            }

            var tableData = await PreviewBulkProductUpdate(columns, bulkProduct);

            return new BulkProductPreviewResult() { IsValid = true, PreviewResult = "", TableData = tableData };
        }

        private async Task<ITableData> PreviewBulkProductUpdate(List<IColumnData> columns, DataTable bulkProdList)
        {
            var bulkProductUpdatePreviewResults = await this._productRepository.BulkUpdatePreview(bulkProdList);
            var updateResultsDict = bulkProductUpdatePreviewResults
                                        .GroupBy(x => x.VendorCode ?? "(No Vendor)")
                                        .ToDictionary(g => g.Key, g => g.ToList());
            var tableData = new TableData();
            tableData.TableGroups = new List<ITableData>();
            var itemGroups = await this._itemGroupService.GetActiveItemGroupsAsync();

            foreach (var item in updateResultsDict)
            {
                var tableGroup = new TableData();
                tableGroup.Columns = columns.ToList();

                tableGroup.Rows = new List<IRowData>();

                foreach (var previewResult in item.Value)
                {
                    var rowData = new RowData
                    {
                        Cells = new Dictionary<string, ICellData>(),
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "row-invalid" : "",
                        Tooltip = string.IsNullOrEmpty(previewResult.Sku) ? "Product does not exists\n" : "",
                    };

                    rowData.Cells.Add(nameof(previewResult.Sku), new CellData()
                    {
                        RawValue = previewResult.Sku,
                        TextValue = previewResult.Sku,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : "",
                        CssIcon = string.IsNullOrEmpty(previewResult.Sku) ? "highlight_off" : ""
                    });

                    rowData.Cells.Add(nameof(previewResult.UPC), new CellData()
                    {
                        RawValue = previewResult.UPC,
                        TextValue = previewResult.UPC,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : "",
                        CssIcon = string.IsNullOrEmpty(previewResult.Sku) ? "highlight_off" : ""
                    });

                    rowData.Cells.Add(nameof(previewResult.CurrentBrand), new CellData()
                    {
                        RawValue = previewResult.CurrentBrand,
                        TextValue = previewResult.CurrentBrand,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                    });
                    rowData.Cells.Add(nameof(previewResult.UpdatedBrand), new CellData()
                    {
                        RawValue = previewResult.UpdatedBrand,
                        TextValue = previewResult.UpdatedBrand,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.BrandChange ? "cell-valid" : "",
                    });

                    rowData.Cells.Add(nameof(previewResult.CurrentStyleCode), new CellData()
                    {
                        RawValue = previewResult.CurrentStyleCode,
                        TextValue = previewResult.CurrentStyleCode,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                    });
                    rowData.Cells.Add(nameof(previewResult.UpdatedStyleCode), new CellData()
                    {
                        RawValue = previewResult.UpdatedStyleCode,
                        TextValue = previewResult.UpdatedStyleCode,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.StyleCodeChange ? "cell-valid" : "",
                    });

                    rowData.Cells.Add(nameof(previewResult.CurrentStyleDesc), new CellData()
                    {
                        RawValue = previewResult.CurrentStyleDesc,
                        TextValue = previewResult.CurrentStyleDesc,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                    });
                    rowData.Cells.Add(nameof(previewResult.UpdatedStyleDesc), new CellData()
                    {
                        RawValue = previewResult.UpdatedStyleDesc,
                        TextValue = previewResult.UpdatedStyleDesc,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.StyleDescChange ? "cell-valid" : "",
                    });

                    rowData.Cells.Add(nameof(previewResult.CurrentSize), new CellData()
                    {
                        RawValue = previewResult.CurrentSize,
                        TextValue = previewResult.CurrentSize,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                    });
                    rowData.Cells.Add(nameof(previewResult.UpdatedSize), new CellData()
                    {
                        RawValue = previewResult.UpdatedSize,
                        TextValue = previewResult.UpdatedSize,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.SizeChange ? "cell-valid" : "",
                    });

                    rowData.Cells.Add(nameof(previewResult.CurrentColor), new CellData()
                    {
                        RawValue = previewResult.CurrentColor,
                        TextValue = previewResult.CurrentColor,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                    });
                    rowData.Cells.Add(nameof(previewResult.UpdatedColor), new CellData()
                    {
                        RawValue = previewResult.UpdatedColor,
                        TextValue = previewResult.UpdatedColor,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.ColorChange ? "cell-valid" : "",
                    });

                    rowData.Cells.Add(nameof(previewResult.CurrentRetail), new CellData()
                    {
                        RawValue = previewResult.CurrentRetail,
                        TextValue = $"${previewResult.CurrentRetail:F2}",
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                    });
                    rowData.Cells.Add(nameof(previewResult.UpdatedRetail), new CellData()
                    {
                        RawValue = previewResult.UpdatedRetail,
                        TextValue = $"${previewResult.UpdatedRetail:F2}",
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.RetailChange && previewResult.CurrentRetail > previewResult.UpdatedRetail ? "cell-changed" : previewResult.RetailChange && previewResult.CurrentRetail < previewResult.UpdatedRetail ? "cell-valid" : "",
                        Tooltip = previewResult.RetailChange && previewResult.CurrentRetail > previewResult.UpdatedRetail ? "Retail price decrease\n" : previewResult.RetailChange && previewResult.CurrentRetail < previewResult.UpdatedRetail ? "Retail price increase\n" : "",
                    });

                    rowData.Cells.Add(nameof(previewResult.CurrentCost), new CellData()
                    {
                        RawValue = previewResult.CurrentCost,
                        TextValue = $"${previewResult.CurrentCost:F2}",
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                    });
                    rowData.Cells.Add(nameof(previewResult.UpdatedCost), new CellData()
                    {
                        RawValue = previewResult.UpdatedCost,
                        TextValue = $"${previewResult.UpdatedCost:F2}",
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.CostChange && previewResult.CurrentCost > previewResult.UpdatedCost ? "cell-changed" : previewResult.CostChange && previewResult.CurrentCost < previewResult.UpdatedCost ? "cell-valid" : "",
                        Tooltip = previewResult.CostChange && previewResult.CurrentCost > previewResult.UpdatedCost ? "Cost decrease\n" : previewResult.CostChange && previewResult.CurrentCost < previewResult.UpdatedCost ? "Cost increase\n" : "",
                    });

                    if (!string.IsNullOrEmpty(previewResult.Sku))
                    {
                        rowData.Cells.Add(nameof(previewResult.CurrentItmGroup), new CellData()
                        {
                            RawValue = itemGroups.ContainsKey(previewResult.CurrentItmGroup) ? $"{itemGroups[previewResult.CurrentItmGroup].Code} ({itemGroups[previewResult.CurrentItmGroup].Name})" : previewResult.CurrentItmGroup,
                            TextValue = itemGroups.ContainsKey(previewResult.CurrentItmGroup) ? $"{itemGroups[previewResult.CurrentItmGroup].Code} ({itemGroups[previewResult.CurrentItmGroup].Name})" : previewResult.CurrentItmGroup,
                            IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        });
                    }
                    else
                    {
                        rowData.Cells.Add(nameof(previewResult.CurrentItmGroup), new CellData()
                        {
                            RawValue = previewResult.CurrentItmGroup,
                            TextValue = previewResult.CurrentItmGroup,
                            IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                            CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.ItmGroupChange ? "cell-changed" : "",
                        });
                    }

                    rowData.Cells.Add(nameof(previewResult.UpdatedItmGroup), new CellData()
                    {
                        RawValue = itemGroups.ContainsKey(previewResult.UpdatedItmGroup) ? $"{itemGroups[previewResult.UpdatedItmGroup].Code} ({itemGroups[previewResult.UpdatedItmGroup].Name})" : previewResult.UpdatedItmGroup,
                        TextValue = itemGroups.ContainsKey(previewResult.UpdatedItmGroup) ? $"{itemGroups[previewResult.UpdatedItmGroup].Code} ({itemGroups[previewResult.UpdatedItmGroup].Name})" : previewResult.UpdatedItmGroup,
                        IsValid = !string.IsNullOrEmpty(previewResult.Sku),
                        CssClass = string.IsNullOrEmpty(previewResult.Sku) ? "cell-invalid" : previewResult.ItmGroupChange ? "cell-valid" : "",
                    });

                    if (previewResult.CurrentRetail != previewResult.UpdatedRetail || previewResult.CurrentCost != previewResult.UpdatedCost)
                    {
                        rowData.CssClass = "row-changed";
                    }

                    if (rowData.Cells.Values.Any(cell => !cell.IsValid))
                    {
                        // Handle invalid row
                        rowData.IsValid = false;
                        rowData.CssClass = "row-invalid";
                        rowData.Tooltip += string.Join(".", rowData.Cells
                                                    .Where(kv => !kv.Value.IsValid && !string.IsNullOrWhiteSpace(kv.Value.Tooltip))
                                                    .Select(kv => kv.Value.Tooltip)
                                                    .ToList());
                    }

                    tableGroup.Rows.Add(rowData);
                }

                tableGroup.TableName = $"{item.Value.First().VendorName} ({item.Key})";
                tableData.TableGroups.Add(tableGroup);
            }

            return tableData;
        }

        public async Task<List<ISearchProductResult>> ProductSearchByUPCListAsync(List<string> upcList)
        {
            DataTable dataTable = DataTableHelper.StringListParameter(upcList);
            var prodSearchOutput = await _productRepository.ProductSearchByUPCListAsync(dataTable);
            return prodSearchOutput;
        }

        public async Task<ITableData> ProductTransferPreviewAsync(IProductTransferRequest productTransferRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var itemGroups = await this._itemGroupService.GetActiveItemGroupsAsync();

            if (currentUser != null)
            {
                var upcList = await ProductSearchByUPCListAsync(productTransferRequest.UPCList);
                var upcDict = upcList.ToDictionary(x => x.UPC);
                var test = JsonConvert.SerializeObject(upcList);
                var storeList = await _storeService.GetAllStoresAsync();
                var selectedStores = storeList.Where(x => productTransferRequest.StoreCodes.Contains(x.Code)).ToDictionary(x => x.Code);
                var tasks = selectedStores
                                .Select(store => _productsApi.SearchProductFromApiAsync(store.Value.ApiUrl, currentUser!.JwtToken!, productTransferRequest.UPCList))
                                .ToList();

                var results = await Task.WhenAll(tasks);

                var combinedResults = results.SelectMany(r => r).ToList();

                var storeGroup = combinedResults.GroupBy(x => x.Storecode).ToDictionary(x => x.Key, x => x.ToList());

                TableData output = new TableData();


                foreach (var store in storeGroup)
                {
                    TableData tableData = new TableData();
                    tableData.TableName = $"{selectedStores[store.Key].Name} ({selectedStores[store.Key].Code})";
                    tableData.Columns.Add(new ColumnData() { FieldName = "VendorName", Header = "Vendor" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Sku", Header = "Sku" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store Brand", Header = "Store Brand" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ Brand", Header = "HQ Brand" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store StyleCode", Header = "Store StyleCode" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ StyleCode", Header = "HQ StyleCode" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store StyleDesc", Header = "Store ProductDescription" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ StyleDesc", Header = "HQ ProductDescription" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store Size", Header = "Store Size" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ Size", Header = "HQ Size" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store Color", Header = "Store Color" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ Color", Header = "HQ Color" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store Retail", Header = "Store RetailPrice", DataType = ColumnDataType.Money });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ Retail", Header = "HQ RetailPrice", DataType = ColumnDataType.Money });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store Cost", Header = "Store Cost", DataType = ColumnDataType.Money });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ Cost", Header = "HQ Cost", DataType = ColumnDataType.Money });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Store ItmGroup", Header = "Store Category" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "HQ ItmGroup", Header = "HQ Category" });

                    foreach (var row in store.Value)
                    {
                        var rowData = new RowData
                        {
                            Cells = new Dictionary<string, ICellData>(),
                            CssClass = string.IsNullOrEmpty(row.Sku) ? "row-valid" : "",
                            Tooltip = string.IsNullOrEmpty(row.Sku) ? "Product will be created\n" : "",
                            IsNew = string.IsNullOrEmpty(row.Sku)
                        };

                        rowData.Cells.Add(nameof(row.VendorName), new CellData()
                        {
                            RawValue = row.VendorName,
                            TextValue = row.VendorName,
                        });

                        rowData.Cells.Add(nameof(row.Sku), new CellData()
                        {
                            RawValue = row.Sku,
                            TextValue = row.Sku,
                        });

                        rowData.Cells.Add(nameof(row.UPC), new CellData()
                        {
                            RawValue = row.UPC,
                            TextValue = row.UPC,
                            CssClass = string.IsNullOrEmpty(row.Sku) ? "cell-valid" : "",
                        });

                        rowData.Cells.Add("Store Brand", new CellData()
                        {
                            RawValue = row.Brand,
                            TextValue = row.Brand,
                        });
                        rowData.Cells.Add("HQ Brand", new CellData()
                        {
                            RawValue = upcDict[row.UPC].Brand,
                            TextValue = upcDict[row.UPC].Brand,
                            CssClass = upcDict[row.UPC].Brand != row.Brand ? "cell-changed" : "",
                        });

                        rowData.Cells.Add("Store StyleCode", new CellData()
                        {
                            RawValue = row.StyleCode,
                            TextValue = row.StyleCode,
                        });
                        rowData.Cells.Add("HQ StyleCode", new CellData()
                        {
                            RawValue = upcDict[row.UPC].StyleCode,
                            TextValue = upcDict[row.UPC].StyleCode,
                            CssClass = upcDict[row.UPC].StyleCode != row.StyleCode ? "cell-changed" : "",
                        });

                        rowData.Cells.Add("Store StyleDesc", new CellData()
                        {
                            RawValue = row.StyleDesc,
                            TextValue = row.StyleDesc,
                        });
                        rowData.Cells.Add("HQ StyleDesc", new CellData()
                        {
                            RawValue = upcDict[row.UPC].StyleDesc,
                            TextValue = upcDict[row.UPC].StyleDesc,
                            CssClass = upcDict[row.UPC].StyleDesc != row.StyleDesc ? "cell-changed" : "",
                        });

                        rowData.Cells.Add("Store Size", new CellData()
                        {
                            RawValue = row.Size,
                            TextValue = row.Size,
                        });
                        rowData.Cells.Add("HQ Size", new CellData()
                        {
                            RawValue = upcDict[row.UPC].Size,
                            TextValue = upcDict[row.UPC].Size,
                            CssClass = upcDict[row.UPC].Size != row.Size ? "cell-changed" : "",
                        });

                        rowData.Cells.Add("Store Color", new CellData()
                        {
                            RawValue = row.Color,
                            TextValue = row.Color,
                        });
                        rowData.Cells.Add("HQ Color", new CellData()
                        {
                            RawValue = upcDict[row.UPC].Color,
                            TextValue = upcDict[row.UPC].Color,
                            CssClass = upcDict[row.UPC].Color != row.Color ? "cell-changed" : "",
                        });

                        rowData.Cells.Add("Store Retail", new CellData()
                        {
                            RawValue = row.Color,
                            TextValue = $"${row.RetailPrice:F2}",
                            Tooltip = upcDict[row.UPC].RetailPrice != row.RetailPrice ? rowData.IsNew ? "" : $"{nameof(row.RetailPrice)} will be updated" : "",
                        });
                        rowData.Cells.Add("HQ Retail", new CellData()
                        {
                            RawValue = upcDict[row.UPC].RetailPrice,
                            TextValue = $"${upcDict[row.UPC].RetailPrice:F2}",
                            CssClass = upcDict[row.UPC].RetailPrice > row.RetailPrice ? "cell-valid" : upcDict[row.UPC].RetailPrice < row.RetailPrice ? "cell-changed" : "",
                            Tooltip = upcDict[row.UPC].RetailPrice > row.RetailPrice ? "Retail price increase" : upcDict[row.UPC].RetailPrice < row.RetailPrice ? "Retail price decrease" : "",
                        });

                        rowData.Cells.Add("Store Cost", new CellData()
                        {
                            RawValue = row.Cost,
                            TextValue = $"${row.Cost:F2}",
                            Tooltip = upcDict[row.UPC].Cost != row.Cost ? rowData.IsNew ? "" : $"{nameof(row.Cost)} will be updated" : "",
                        });
                        rowData.Cells.Add("HQ Cost", new CellData()
                        {
                            RawValue = upcDict[row.UPC].Cost,
                            TextValue = $"${upcDict[row.UPC].Cost:F2}",
                            CssClass = upcDict[row.UPC].Cost > row.Cost ? "cell-valid" : upcDict[row.UPC].Cost < row.Cost ? "cell-changed" : "",
                            Tooltip = upcDict[row.UPC].Cost > row.Cost ? "Cost increase" : upcDict[row.UPC].Cost < row.Cost ? "Cost decrease" : "",
                        });

                        if (!string.IsNullOrWhiteSpace(row.ItmGroup))
                        {
                            rowData.Cells.Add("Store ItmGroup", new CellData()
                            {
                                RawValue = itemGroups.ContainsKey(row.ItmGroup) ? $"{itemGroups[row.ItmGroup].Code} ({itemGroups[row.ItmGroup].Name})" : row.ItmGroup,
                                TextValue = itemGroups.ContainsKey(row.ItmGroup) ? $"{itemGroups[row.ItmGroup].Code} ({itemGroups[row.ItmGroup].Name})" : row.ItmGroup,
                            });
                        }
                        else
                        {
                            rowData.Cells.Add("Store ItmGroup", new CellData()
                            {
                                RawValue = "",
                                TextValue = "",
                                CssClass = "",
                            });
                        }
                        rowData.Cells.Add("HQ ItmGroup", new CellData()
                        {
                            RawValue = itemGroups.ContainsKey(upcDict[row.UPC].ItmGroup) ? $"{itemGroups[upcDict[row.UPC].ItmGroup].Code} ({itemGroups[upcDict[row.UPC].ItmGroup].Name})" : upcDict[row.UPC].ItmGroup,
                            TextValue = itemGroups.ContainsKey(upcDict[row.UPC].ItmGroup) ? $"{itemGroups[upcDict[row.UPC].ItmGroup].Code} ({itemGroups[upcDict[row.UPC].ItmGroup].Name})" : upcDict[row.UPC].ItmGroup,
                            CssClass = upcDict[row.UPC].ItmGroup != row.ItmGroup ? "cell-changed" : "",
                        });

                        // Check changed values
                        if (!rowData.IsNew && (upcDict[row.UPC].Cost != row.Cost || upcDict[row.UPC].RetailPrice != row.RetailPrice))
                        {
                            rowData.CssClass = "row-changed";
                            rowData.IsNew = false;
                            rowData.IsChanged = true;
                        }

                        if (rowData.IsNew || rowData.IsChanged)
                        {
                            tableData.Rows.Add(rowData);
                        }
                    }

                    output.TableGroups.Add(tableData);
                }

                return output;
            }


            return new TableData();
        }

        public async Task<IProductTransferResult> TransferProductsAsync(List<ISearchProductResult> productTransferRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            IProductTransferResult productTransferResult = new ProductTransferResult();
            DateTime requestStart = DateTime.Now;

            if (currentUser != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Bulk Product Transfer by {currentUser?.Claims["UserCode"]!}");
                sb.AppendLine($"Bulk Product Transfer Start: {requestStart}");
                try
                {
                    var upcTable = DataTableHelper.ToProductTransferDetailsDataTable(productTransferRequest);
                    var upcDict = productTransferRequest.ToDictionary(x => x.UPC);

                    var result = await _productRepository.TransferProductsAsync(new TransferProductParams() { UserCode = currentUser?.Claims["UserCode"]!, ProductTransferDetails = upcTable });
                    var store = await _systemSetupService.GetSystemSetupAsync();
                    TableData output = new TableData();

                    TableData tableData = new TableData();
                    tableData.TableName = $"{store["B"].AltValue} ({store["B"].Description})";
                    tableData.Columns.Add(new ColumnData() { FieldName = "VendorName", Header = "Vendor" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Sku", Header = "Sku" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UPC", Header = "UPC" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentBrand", Header = "Previous Brand" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedBrand", Header = "Updated Brand" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentStyleCode", Header = "Previous StyleCode" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedStyleCode", Header = "Updated StyleCode" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentStyleDesc", Header = "Previous ProductDescription" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedStyleDesc", Header = "Updated ProductDescription" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentSize", Header = "Previous Size" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedSize", Header = "Updated Size" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentColor", Header = "Previous Color" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedColor", Header = "Updated Color" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentRetail", Header = "Previous Retail" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedRetail", Header = "Updated Retail" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentCost", Header = "Previous Cost" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedCost", Header = "Updated Cost" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "CurrentItmGroup", Header = "Previous Category" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "UpdatedItmGroup", Header = "Updated Category" });
                    tableData.Columns.Add(new ColumnData() { FieldName = "Result", Header = "Result" });

                    foreach (var transferResult in result)
                    {
                        var rowData = new RowData
                        {
                            Cells = new Dictionary<string, ICellData>(),
                            CssClass = transferResult.IsNew ? "row-valid" : "row-changed",
                        };


                        rowData.Cells.Add(nameof(transferResult.VendorName), new CellData()
                        {
                            RawValue = transferResult.VendorName,
                            TextValue = transferResult.VendorName,
                        });

                        rowData.Cells.Add(nameof(transferResult.Sku), new CellData()
                        {
                            RawValue = transferResult.Sku,
                            TextValue = transferResult.Sku,
                        });

                        rowData.Cells.Add(nameof(transferResult.UPC), new CellData()
                        {
                            RawValue = transferResult.UPC,
                            TextValue = transferResult.UPC,
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentBrand), new CellData()
                        {
                            RawValue = transferResult.CurrentBrand,
                            TextValue = transferResult.CurrentBrand,
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedBrand), new CellData()
                        {
                            RawValue = transferResult.UpdatedBrand,
                            TextValue = transferResult.UpdatedBrand,
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentStyleCode), new CellData()
                        {
                            RawValue = transferResult.CurrentStyleCode,
                            TextValue = transferResult.CurrentStyleCode,
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedStyleCode), new CellData()
                        {
                            RawValue = transferResult.UpdatedStyleCode,
                            TextValue = transferResult.UpdatedStyleCode,
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentStyleDesc), new CellData()
                        {
                            RawValue = transferResult.CurrentStyleDesc,
                            TextValue = transferResult.CurrentStyleDesc,
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedStyleDesc), new CellData()
                        {
                            RawValue = transferResult.UpdatedStyleDesc,
                            TextValue = transferResult.UpdatedStyleDesc,
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentSize), new CellData()
                        {
                            RawValue = transferResult.CurrentSize,
                            TextValue = transferResult.CurrentSize,
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedSize), new CellData()
                        {
                            RawValue = transferResult.UpdatedSize,
                            TextValue = transferResult.UpdatedSize,
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentColor), new CellData()
                        {
                            RawValue = transferResult.CurrentColor,
                            TextValue = transferResult.CurrentColor,
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedColor), new CellData()
                        {
                            RawValue = transferResult.UpdatedColor,
                            TextValue = transferResult.UpdatedColor,
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentRetail), new CellData()
                        {
                            RawValue = transferResult.CurrentRetail,
                            TextValue = $"${transferResult.CurrentRetail:F2}",
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedRetail), new CellData()
                        {
                            RawValue = transferResult.UpdatedRetail,
                            TextValue = $"${transferResult.UpdatedRetail:F2}",
                            CssClass = transferResult.UpdatedRetail > transferResult.CurrentRetail ? "cell-valid" : transferResult.UpdatedRetail < transferResult.CurrentRetail ? "cell-changed" : "",
                            Tooltip = transferResult.UpdatedRetail > transferResult.CurrentRetail ? "Retail price increase" : transferResult.UpdatedRetail < transferResult.CurrentRetail ? "Retail price decrease" : "",
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentCost), new CellData()
                        {
                            RawValue = transferResult.CurrentCost,
                            TextValue = $"${transferResult.CurrentCost:F2}",
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedCost), new CellData()
                        {
                            RawValue = transferResult.UpdatedCost,
                            TextValue = $"${transferResult.UpdatedCost:F2}",
                            CssClass = transferResult.UpdatedCost > transferResult.CurrentCost ? "cell-valid" : transferResult.UpdatedCost < transferResult.CurrentCost ? "cell-changed" : "",
                            Tooltip = transferResult.UpdatedCost > transferResult.CurrentCost ? "Cost increase" : transferResult.UpdatedCost < transferResult.CurrentCost ? "Cost decrease" : "",
                        });

                        rowData.Cells.Add(nameof(transferResult.CurrentItmGroup), new CellData()
                        {
                            RawValue = transferResult.CurrentItmGroup,
                            TextValue = transferResult.CurrentItmGroup,
                        });
                        rowData.Cells.Add(nameof(transferResult.UpdatedItmGroup), new CellData()
                        {
                            RawValue = transferResult.UpdatedItmGroup,
                            TextValue = transferResult.UpdatedItmGroup,
                        });
                        rowData.Cells.Add(nameof(transferResult.Result), new CellData()
                        {
                            RawValue = transferResult.Result,
                            TextValue = transferResult.Result,
                        });

                        tableData.Rows.Add(rowData);
                    }

                    output.TableGroups.Add(tableData);

                    sb.AppendLine($"Bulk Product Transfer Store Count : {tableData.TableName} - {tableData.Rows.Count}");
                    sb.AppendLine($"Bulk Product Transfer End: {DateTime.Now}");
                    _logger.LogInformation(sb.ToString());

                    return new ProductTransferResult() { StoreCode = store["B"].Value, StartTime = requestStart, EndTime = DateTime.Now, TableData = output, TransferResult = result };
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Bulk Product Transfer Error : {ex.Message}");
                    sb.AppendLine($"Bulk Product Transfer End: {DateTime.Now}");
                    _logger.LogInformation(sb.ToString());
                    return new ProductTransferResult();
                }
            }
            return new ProductTransferResult();
        }
        public async Task<ITableData> ProductTransferToStoresAsync(IProductTransferRequest productTransferRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            DateTime requestStart = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Bulk Product Transfer Request by {currentUser?.Claims["UserCode"]!}");
            sb.AppendLine($"Bulk Product Transfer Request Start: {requestStart}");

            if (currentUser != null)
            {
                try
                {
                    var upcList = await ProductSearchByUPCListAsync(productTransferRequest.UPCList);
                    var upcDict = upcList.ToDictionary(x => x.UPC);
                    var jsonVal = JsonConvert.SerializeObject(upcDict);
                    var storeList = await _storeService.GetAllStoresAsync();
                    var selectedStores = storeList.Where(x => productTransferRequest.StoreCodes.Contains(x.Code)).ToDictionary(x => x.Code);
                    var tasks = selectedStores
                                    .Select(store => _productsApi.TransferProductsToApiAsync<ProductTransferApiResult>(store.Value.ApiUrl, currentUser!.JwtToken!, upcList))
                                    .ToList();

                    var results = await Task.WhenAll(tasks);

                    var combinedResults = results.Select(r => r).ToList();

                    TableData output = new TableData();
                    foreach (var item in combinedResults)
                    {
                        var logProductTransfersParam = new LogProductTransfersParam();
                        logProductTransfersParam.UserCode = currentUser?.Claims["UserCode"]!;
                        logProductTransfersParam.StoreCode = item.StoreCode;
                        logProductTransfersParam.StartTime = item.StartTime;
                        logProductTransfersParam.EndTime = item.EndTime;
                        logProductTransfersParam.RequestDate = requestStart;
                        logProductTransfersParam.IsSuccessful = true;
                        logProductTransfersParam.IsScheduled = false;
                        logProductTransfersParam.IsNew.Columns.AddRange(new DataColumn[]
                                                                        {
                                                                            new DataColumn { ColumnName = "NameValue" },
                                                                            new DataColumn { ColumnName = "Value" }
                                                                        });
                        logProductTransfersParam.CurrentRetailPrice.Columns.AddRange(new DataColumn[]
                                                                        {
                                                                            new DataColumn { ColumnName = "NameValue" },
                                                                            new DataColumn { ColumnName = "Value" }
                                                                        });
                        logProductTransfersParam.NewRetailPrice.Columns.AddRange(new DataColumn[]
                                                                        {
                                                                            new DataColumn { ColumnName = "NameValue" },
                                                                            new DataColumn { ColumnName = "Value" }
                                                                        });
                        logProductTransfersParam.CurrentCost.Columns.AddRange(new DataColumn[]
                                                                        {
                                                                            new DataColumn { ColumnName = "NameValue" },
                                                                            new DataColumn { ColumnName = "Value" }
                                                                        });
                        logProductTransfersParam.NewCost.Columns.AddRange(new DataColumn[]
                                                                        {
                                                                            new DataColumn { ColumnName = "NameValue" },
                                                                            new DataColumn { ColumnName = "Value" }
                                                                        });

                        foreach (var itemData in item.TransferResult)
                        {
                            var newDataRow = logProductTransfersParam.IsNew.NewRow();
                            var currentRetailDataRow = logProductTransfersParam.CurrentRetailPrice.NewRow();
                            var newRetailDataRow = logProductTransfersParam.NewRetailPrice.NewRow();
                            var currentCostDataRow = logProductTransfersParam.CurrentCost.NewRow();
                            var newCostDataRow = logProductTransfersParam.NewCost.NewRow();
                            logProductTransfersParam.IsNew.Rows.Add(itemData.UPC, itemData.IsNew.ToString());
                            logProductTransfersParam.CurrentRetailPrice.Rows.Add(itemData.UPC, itemData.CurrentRetail.ToString());
                            logProductTransfersParam.NewRetailPrice.Rows.Add(itemData.UPC, itemData.UpdatedRetail.ToString());
                            logProductTransfersParam.CurrentCost.Rows.Add(itemData.UPC, itemData.CurrentCost.ToString());
                            logProductTransfersParam.NewCost.Rows.Add(itemData.UPC, itemData.UpdatedCost.ToString());
                        }

                        foreach (var itemTable in item.TableData.TableGroups)
                        {
                            var tableData = _mapper.Map<TableData>(itemTable);
                            output.TableGroups.Add(tableData);
                            sb.AppendLine($"Bulk Product Transfer Request Store Count : {tableData.TableName} - {tableData.Rows.Count}");
                        }
                        logProductTransfersParam.Result = "Transferred Successfully";
                        await _productRepository.LogProductTransfersAsync(logProductTransfersParam);
                    }
                    sb.AppendLine($"Bulk Product Transfer Request End: {DateTime.Now}");
                    _logger.LogInformation(sb.ToString());
                    return output;
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Bulk Product Transfer Request Error: {ex.Message}");
                    sb.AppendLine($"Bulk Product Transfer Request End: {DateTime.Now}");
                    _logger.LogError(sb.ToString());
                    return new TableData();
                }
            }
            sb.AppendLine($"Bulk Product Transfer Request End: {DateTime.Now}");
            _logger.LogInformation(sb.ToString());

            return new TableData();
        }
        private List<BulkProductResultDto> DatatableToBulkProductDataRequest(DataTable dataTable)
        {
            List<BulkProductResultDto> output = new List<BulkProductResultDto>();

            foreach (DataRow dr in dataTable.Rows)
            {
                output.Add(new BulkProductResultDto()
                {
                    Brand = dr[0].ToString(),
                    StyleCode = dr[1].ToString(),
                    StyleDesc = dr[2].ToString(),
                    Size = dr[3].ToString(),
                    Color = dr[4].ToString(),
                    Retail = dr[5].ToString(),
                    Cost = dr[6].ToString(),
                    ItmGroup = dr[7].ToString(),
                    UPC = dr[8].ToString()
                });
            }
            return output;
        }
        public async Task<ITableData> SearchProductsAsync(IProductSearchParams searchParams)
        {
            try
            {
                var output = await _productRepository.SearchProductsAsync(searchParams);
                var tableData = new TableData();
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Vendor Name",
                    FieldName = "Vendor Name",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Brand",
                    FieldName = "Brand",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Style Code",
                    FieldName = "Style Code",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Product Description",
                    FieldName = "Product Description",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Size",
                    FieldName = "Size",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Color",
                    FieldName = "Color",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Retail Price",
                    FieldName = "Retail Price",
                    DataType = ColumnDataType.Decimal
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Cost",
                    FieldName = "Cost",
                    DataType = ColumnDataType.Decimal
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Category",
                    FieldName = "Category",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "UPC",
                    FieldName = "UPC",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "SKU",
                    FieldName = "Sku",
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Write Date",
                    FieldName = "Write Date",
                    DataType = ColumnDataType.Date
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Update Date",
                    FieldName = "Update Date",
                    DataType = ColumnDataType.Date
                });

                tableData.Columns.Add(new ColumnData()
                {
                    Header = "View Promotions",
                    FieldName = "Promotions",
                    DataType = ColumnDataType.Int,
                    IsCommand = true,
                    CommandName = "getPromotions"
                });
                foreach (var row in output)
                {
                    var rowData = new RowData()
                    {
                        Cells = new Dictionary<string, ICellData>(),
                    };

                    rowData.Cells.Add("Vendor Name", new CellData()
                    {
                        RawValue = row.VendorName,
                        TextValue = row.VendorName,
                    });
                    rowData.Cells.Add(nameof(row.Brand), new CellData()
                    {
                        RawValue = row.Brand,
                        TextValue = row.Brand,
                    });
                    rowData.Cells.Add("Style Code", new CellData()
                    {
                        RawValue = row.StyleCode,
                        TextValue = row.StyleCode,
                    });
                    rowData.Cells.Add("Product Description", new CellData()
                    {
                        RawValue = row.StyleDesc,
                        TextValue = row.StyleDesc,
                    });
                    rowData.Cells.Add(nameof(row.Size), new CellData()
                    {
                        RawValue = row.Size,
                        TextValue = row.Size,
                    });
                    rowData.Cells.Add(nameof(row.Color), new CellData()
                    {
                        RawValue = row.Color,
                        TextValue = row.Color,
                    });
                    rowData.Cells.Add("Retail Price", new CellData()
                    {
                        RawValue = row.RetailPrice,
                        TextValue = $"${row.RetailPrice:F2}",
                    });
                    rowData.Cells.Add(nameof(row.Cost), new CellData()
                    {
                        RawValue = row.Cost,
                        TextValue = $"${row.Cost:F2}",
                    });
                    rowData.Cells.Add(nameof(row.Category), new CellData()
                    {
                        RawValue = row.Category,
                        TextValue = row.Category,
                    });
                    rowData.Cells.Add(nameof(row.UPC), new CellData()
                    {
                        RawValue = row.UPC,
                        TextValue = row.UPC,
                    });
                    rowData.Cells.Add(nameof(row.Sku), new CellData()
                    {
                        RawValue = row.Sku,
                        TextValue = row.Sku,
                    });
                    rowData.Cells.Add("Write Date", new CellData()
                    {
                        RawValue = row.WriteDate,
                        TextValue = row.WriteDate?.ToShortDateString(),
                    });
                    rowData.Cells.Add("Update Date", new CellData()
                    {
                        RawValue = row.LastUpdate,
                        TextValue = row.LastUpdate?.ToShortDateString(),
                    });
                    rowData.Cells.Add("Promotions", new CellData()
                    {
                        RawValue = "View Promotions",
                        TextValue = "View Promotions",
                        CommandParameter = new { product = row },
                        Tooltip = "View Promotions",
                        CssClass = "cell-center"

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
