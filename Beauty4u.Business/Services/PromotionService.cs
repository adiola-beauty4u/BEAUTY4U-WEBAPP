using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Beauty4u.ApiAccess.Products;
using Beauty4u.Common.Enums;
using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.DataAccess.Api;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.Promotions;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.Api.Promotions;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto.Promotions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Beauty4u.Business.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly ILogger<PromotionService> _logger;
        private readonly IStoreService _storeService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPromotionsApi _promotionsApi;
        private readonly IMapper _mapper;
        private readonly bool _isHq;
        private readonly string _hqApi;
        public PromotionService(IConfiguration configuration, IPromotionRepository promotionRepository,
            ILogger<PromotionService> logger, ICurrentUserService currentUserService,
            IPromotionsApi promotionsApi, IMapper mapper, IStoreService storeService)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;
            _currentUserService = currentUserService;
            _isHq = configuration.GetValue<bool>("HqSettings:IsHq");
            _hqApi = configuration.GetValue<string>("HqSettings:HqApi")!;
            _promotionsApi = promotionsApi;
            _mapper = mapper;
            _storeService = storeService;
        }

        public async Task<ITableData> GetProductPromotionsBySkuAsync(string sku)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
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
                            TextValue = $"${row.Cost:N2}",
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.RetailPrice), new CellData()
                        {
                            RawValue = row.RetailPrice,
                            TextValue = $"${row.RetailPrice:N2}",
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.NewPrice), new CellData()
                        {
                            RawValue = row.NewPrice,
                            TextValue = $"${row.NewPrice:N2}",
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
            return new TableData();
        }
        public async Task<ITableData> GetProductPromotionsByPromoNoAsync(IGetProductPromotionRequest getProductPromotionRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                try
                {
                    var data = await _promotionRepository.GetProductPromotionsByPromoNoAsync(getProductPromotionRequest.PromoNo);

                    var tableData = new TableData();
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Vendor",
                        FieldName = "Vendor",
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Brand",
                        FieldName = nameof(ProductPromotion.Brand),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Style Code",
                        FieldName = nameof(ProductPromotion.StyleCode),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Product Description",
                        FieldName = nameof(ProductPromotion.StyleDesc),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Category",
                        FieldName = nameof(ProductPromotion.Category),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Color",
                        FieldName = nameof(ProductPromotion.Color),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Size",
                        FieldName = nameof(ProductPromotion.Size),
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
                        Header = "Current Retail Price",
                        FieldName = nameof(ProductPromotion.CurrentRetailPrice),
                        DataType = ColumnDataType.Decimal
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "New Promotion Price",
                        FieldName = nameof(ProductPromotion.NewPrice),
                        DataType = ColumnDataType.Decimal
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Sku",
                        FieldName = nameof(ProductPromotion.Sku),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "UPC",
                        FieldName = nameof(ProductPromotion.UPC),
                        DataType = ColumnDataType.String
                    });
                    if (getProductPromotionRequest.FromPromoPage)
                    {
                        tableData.Columns.Add(new ColumnData()
                        {
                            Header = "Rule",
                            FieldName = "Rule",
                            DataType = ColumnDataType.Bool
                        });
                    }
                    else
                    {
                        tableData.Columns.Add(new ColumnData()
                        {
                            Header = "View Promotions",
                            FieldName = "Promotions",
                            DataType = ColumnDataType.Int,
                            SlideInCommand = "getPromotions"
                        });
                    }
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Product",
                        FieldName = "Product",
                        DataType = ColumnDataType.String,
                        IsSlideInColumn = true,
                        IsHidden = true
                    });

                    foreach (ProductPromotion row in data)
                    {
                        var rowData = new RowData()
                        {
                            Cells = new Dictionary<string, ICellData>(),
                        };

                        rowData.Cells.Add("Vendor", new CellData()
                        {
                            RawValue = row.VendorName,
                            TextValue = row.VendorName,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.Brand), new CellData()
                        {
                            RawValue = row.Brand,
                            TextValue = row.Brand,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.StyleCode), new CellData()
                        {
                            RawValue = row.StyleCode,
                            TextValue = row.StyleCode,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.StyleDesc), new CellData()
                        {
                            RawValue = row.StyleDesc,
                            TextValue = row.StyleDesc,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.Category), new CellData()
                        {
                            RawValue = row.Category,
                            TextValue = row.Category,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.Color), new CellData()
                        {
                            RawValue = row.Color,
                            TextValue = row.Color,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.Size), new CellData()
                        {
                            RawValue = row.Size,
                            TextValue = row.Size,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.Cost), new CellData()
                        {
                            RawValue = row.Cost,
                            TextValue = $"${row.Cost:N2}",
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.CurrentRetailPrice), new CellData()
                        {
                            RawValue = row.CurrentRetailPrice,
                            TextValue = $"${row.CurrentRetailPrice:N2}",
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.NewPrice), new CellData()
                        {
                            RawValue = row.NewPrice,
                            TextValue = $"${row.NewPrice:N2}",
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.Sku), new CellData()
                        {
                            RawValue = row.Sku,
                            TextValue = row.Sku,
                        });
                        rowData.Cells.Add(nameof(ProductPromotion.UPC), new CellData()
                        {
                            RawValue = row.UPC,
                            TextValue = row.UPC,
                        });
                        if (getProductPromotionRequest.FromPromoPage)
                        {
                            rowData.Cells.Add("Rule", new CellData()
                            {
                                RawValue = row.PromoRuleId.HasValue,
                                TextValue = row.PromoRuleId.HasValue.ToString(),
                            });
                        }
                        else
                        {
                            rowData.Cells.Add("Promotions", new CellData()
                            {
                                RawValue = "View Promotions",
                                TextValue = "View Promotions",
                                SlideInCommandParameter = new { product = row },
                                Tooltip = "View Promotions",
                                CssClass = "cell-center"
                            });
                        }

                        rowData.Cells.Add("Product", new CellData()
                        {
                            RawValue = row.Sku,
                            TextValue = $"{row.StyleDesc} - (Sku: {row.Sku}, UPC: {row.UPC})",
                        });

                        rowData.AdditionalData = new { sku = row.Sku, newPrice = row.NewPrice, cost = row.Cost, retailPrice = row.RetailPrice, promoRuleId = row.PromoRuleId };

                        if (row.PromoRuleId.HasValue)
                        {
                            string dcPrefix = "";
                            string dcSuffix = "";
                            int dcMultiplier = 1;
                            if (row.RulePromoType.Contains("%"))
                            {
                                dcSuffix = "%";
                                dcMultiplier = 100;
                            }
                            if (row.RulePromoType.Contains("$"))
                                dcPrefix = "$";

                            rowData.GroupValue = $"RuleType - {row.RulePromoType}={dcPrefix}{(row.RulePromoRate * dcMultiplier):N2}{dcSuffix};";
                            if (!string.IsNullOrWhiteSpace(row.RuleVendorName))
                            {
                                rowData.GroupValue += $"Vendor={row.RuleVendorName};";
                            }
                            if (!string.IsNullOrWhiteSpace(row.RuleCategory))
                            {
                                rowData.GroupValue += $"Category={row.RuleCategory};";
                            }
                            if (!string.IsNullOrWhiteSpace(row.RuleBrand))
                            {
                                rowData.GroupValue += $"Brand={row.RuleBrand};";
                            }
                            if (!string.IsNullOrWhiteSpace(row.RuleRetailPrice))
                            {
                                rowData.GroupValue += $"RetailPrice={row.RuleRetailPrice};";
                            }
                        }

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
            return new TableData();
        }
        public async Task<IPromotionDto> GetByPromoNoAsync(string promoNo)
        {
            return await _promotionRepository.GetByPromoNoAsync(promoNo);
        }
        public async Task<ITableData> SearchPromotionsAsync(IPromoSearchParams promoSearchParams)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                try
                {
                    var data = await _promotionRepository.SearchPromotionsAsync(promoSearchParams);

                    var tableData = new TableData();
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Promo No",
                        FieldName = nameof(PromotionDto.PromoNo),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Promo Name",
                        FieldName = nameof(PromotionDto.PromoName),
                        DataType = ColumnDataType.String,
                        IsSlideInColumn = true,
                        SlideInCommand = "viewItems",
                        SlideInTitle = "View Promo Items"
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Promo Date",
                        FieldName = nameof(PromotionDto.PromoDate),
                        DataType = ColumnDataType.Date
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Start Date",
                        FieldName = nameof(PromotionDto.StartDate),
                        DataType = ColumnDataType.Date
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "End Date",
                        FieldName = nameof(PromotionDto.EndDate),
                        DataType = ColumnDataType.Date
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Promo Type",
                        FieldName = nameof(PromotionDto.PromoType),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Status",
                        FieldName = nameof(PromotionDto.Status),
                        DataType = ColumnDataType.Bool
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Is Active",
                        FieldName = nameof(PromotionDto.IsActive),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "DC",
                        FieldName = nameof(PromotionDto.DC),
                        DataType = ColumnDataType.Decimal
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Final Sale",
                        FieldName = nameof(PromotionDto.FinalSale),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Store List",
                        FieldName = nameof(PromotionDto.StoreList),
                        DataType = ColumnDataType.String
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Write Date",
                        FieldName = nameof(PromotionDto.WriteDate),
                        DataType = ColumnDataType.Date
                    });
                    tableData.Columns.Add(new ColumnData()
                    {
                        Header = "Last Update",
                        FieldName = nameof(PromotionDto.LastUpdate),
                        DataType = ColumnDataType.Date
                    });

                    foreach (var row in data)
                    {
                        var rowData = new RowData()
                        {
                            Cells = new Dictionary<string, ICellData>(),
                        };

                        rowData.Cells.Add(nameof(PromotionDto.PromoNo), new CellData()
                        {
                            RawValue = row.PromoNo,
                            TextValue = row.PromoNo,
                        });
                        rowData.Cells.Add(nameof(PromotionDto.PromoName), new CellData()
                        {
                            RawValue = row.PromoName,
                            TextValue = row.PromoName,
                            SlideInCommandParameter = new { promo = row },
                            Tooltip = "View Promo"
                        });
                        rowData.Cells.Add(nameof(PromotionDto.PromoDate), new CellData()
                        {
                            RawValue = row.PromoDate,
                            TextValue = row.PromoDate,
                        });
                        rowData.Cells.Add(nameof(PromotionDto.StartDate), new CellData()
                        {
                            RawValue = row.StartDate,
                            TextValue = row.StartDate,
                        });
                        rowData.Cells.Add(nameof(PromotionDto.EndDate), new CellData()
                        {
                            RawValue = row.EndDate,
                            TextValue = row.EndDate,
                        });
                        rowData.Cells.Add(nameof(PromotionDto.PromoType), new CellData()
                        {
                            RawValue = row.PromoTypeName,
                            TextValue = row.PromoTypeName,
                        });
                        rowData.Cells.Add(nameof(PromotionDto.Status), new CellData()
                        {
                            RawValue = row.Status,
                            TextValue = row.Status.ToString(),
                        });
                        rowData.Cells.Add(nameof(PromotionDto.IsActive), new CellData()
                        {
                            RawValue = row.IsActive,
                            TextValue = row.IsActive,
                        });

                        string dcPrefix = "";
                        string dcSuffix = "";
                        int dcMultiplier = 1;
                        if (row.PromoTypeName.Contains("%"))
                        {
                            dcSuffix = "%";
                            dcMultiplier = 100;
                        }
                        if (row.PromoTypeName.Contains("$"))
                            dcPrefix = "$";

                        rowData.Cells.Add(nameof(PromotionDto.DC), new CellData()
                        {
                            RawValue = row.DC,
                            TextValue = $"{dcPrefix}{(row.DC * dcMultiplier):N2}{dcSuffix}",
                        });
                        rowData.Cells.Add(nameof(PromotionDto.FinalSale), new CellData()
                        {
                            RawValue = row.FinalSale,
                            TextValue = row.FinalSale,
                        });
                        rowData.Cells.Add(nameof(PromotionDto.StoreList), new CellData()
                        {
                            RawValue = row.StoreList,
                            TextValue = row.StoreList,
                        });
                        rowData.Cells.Add(nameof(PromotionDto.WriteDate), new CellData()
                        {
                            RawValue = row.WriteDate,
                            TextValue = row.WriteDate.ToString("yyyy-MM-dd"),
                        });
                        rowData.Cells.Add(nameof(PromotionDto.LastUpdate), new CellData()
                        {
                            RawValue = row.LastUpdate,
                            TextValue = row.LastUpdate.ToString("yyyy-MM-dd"),
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
            return new TableData();
        }
        public async Task CreatePromotionAsync(IPromotionRequest promotionCreateRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                try
                {
                    promotionCreateRequest.CurrentUser = currentUser?.Claims["UserCode"]!;
                    if (promotionCreateRequest.PromoType == "B" || promotionCreateRequest.PromoType == "G")
                    {
                        promotionCreateRequest.SumQty = 2;
                    }
                    await _promotionRepository.CreatePromotionAsync(promotionCreateRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        public async Task UpdatePromotionAsync(IPromotionRequest promotionRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                try
                {
                    promotionRequest.CurrentUser = currentUser?.Claims["UserCode"]!;
                    if (promotionRequest.PromoType == "B" || promotionRequest.PromoType == "G")
                    {
                        promotionRequest.SumQty = 2;
                    }
                    await _promotionRepository.UpdatePromotionAsync(promotionRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        public async Task TransferPromoAsync(IPromotionRequest promotionRequest)
        {
            await _promotionRepository.TransferPromoAsync(promotionRequest);
        }
        public async Task UpdatePromoStoreAsync(IPromoTransferRequest promotionRequest)
        {
            await _promotionRepository.UpdatePromoStoreAsync(promotionRequest);
        }
        public async Task<List<string>> TransferPromoToStoresAsync(IPromoTransferRequest promoTransferRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var requestStart = DateTime.Now;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Promotion Transfer Request by {currentUser?.Claims["UserCode"]!}");
            sb.AppendLine($"Promotion Transfer Request Start: {requestStart}");

            var transferRequest = (PromoTransferRequest)promoTransferRequest;

            if (currentUser != null)
            {
                try
                {
                    var promo = (PromotionDto)await GetByPromoNoAsync(promoTransferRequest.PromoNo);
                    if (promo != null)
                    {
                        var items = await _promotionRepository.GetProductPromotionsByPromoNoAsync(promoTransferRequest.PromoNo);
                        var promoRules = await _promotionRepository.GetPromoRulesByPromoNoAsync(promoTransferRequest.PromoNo);
                        var storeList = await _storeService.GetAllStoresAsync();
                        var storeDict = storeList.ToDictionary(x => x.Code);
                        var selectedStores = storeList.Where(x => promoTransferRequest.StoreCodes.Contains(x.Code)).ToDictionary(x => x.Code);
                        var tasks = selectedStores
                                        .Select(store =>
                                        {
                                            var promoRequest = _mapper.Map<PromotionRequest>(promo);
                                            promoRequest.PromotionRules = promoRules.Cast<PromotionRuleDto>().ToList();

                                            promoRequest.PromotionItems = items.Select(x => new PromotionItems()
                                            {
                                                Cost = x.Cost,
                                                Price = x.NewPrice,
                                                PromoRuleId = x.PromoRuleId,
                                                RetailPrice = x.RetailPrice,
                                                Sku = x.Sku
                                            }).ToList();
                                            promoRequest.StoreCode = store.Key;
                                            return _promotionsApi.TransferPromoToStoreAsync<PromoTransferResult>(store.Value.ApiUrl, currentUser!.JwtToken!, promoRequest);
                                        }).ToList();

                        var results = await Task.WhenAll(tasks);

                        var combinedResults = results.Select(r => r).ToList();

                        var successfulStoreTransfers = combinedResults.Where(x => x.IsSuccess).SelectMany(x => x.StoreCodes.Select(y => y.Value)).Distinct().ToList();

                        var promoStoreUpdate = new PromoTransferRequest()
                        {
                            PromoNo = promoTransferRequest.PromoNo,
                            StoreCodes = successfulStoreTransfers
                        };

                        await UpdatePromoStoreAsync(promoStoreUpdate);

                        var promoStoreTasks = storeList.Where(x => !string.IsNullOrWhiteSpace(x.ApiUrl)).Select(
                                                store =>
                                                {
                                                    return _promotionsApi.UpdatePromoStoreAsync<PromoTransferResult>(store.ApiUrl, currentUser!.JwtToken!, promoStoreUpdate);
                                                })
                                        .ToList();

                        var promoStoreResults = await Task.WhenAll(tasks);

                        var updatedStores = selectedStores.Values.Where(x => successfulStoreTransfers.Contains(x.Code)).Select(x => x.StoreAbbr).ToList();

                        sb.AppendLine($"Promotion Transfer Request End: {DateTime.Now}");
                        _logger.LogInformation(sb.ToString());
                        return updatedStores;
                    }
                    return new List<string>();
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Promotion Transfer Request Error: {ex.Message}");
                    sb.AppendLine($"Promotion Transfer Request End: {DateTime.Now}");
                    _logger.LogError(sb.ToString());
                    throw;
                }
            }
            else
            {
                return new List<string>();
            }

        }

        public async Task TransferAllPromoToStoresAsync()
        {
            var promoSearchParams = new PromoSearchParams();
            promoSearchParams.FromDate = DateTime.Now;
            promoSearchParams.Status = "active";

            var data = await _promotionRepository.SearchPromotionsAsync(promoSearchParams);
            data.Select(promo =>
            {

            });

        }
    }
}
