using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Beauty4u.Common.Helpers;
using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.Promotions;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.Api.Promotions;
using Beauty4u.Models.Dto.Products;
using Beauty4u.Models.Dto.Promotions;
using Beauty4u.Models.Dto.Stores;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Beauty4u.DataAccess.B4u
{
    public class B4uPromotionRepository : IPromotionRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        public B4uPromotionRepository(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("B4uConnection")
            ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing");
            _mapper = mapper;
        }

        public async Task<List<IProductPromotion>> GetProductPromotionsBySkuAsync(string sku)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_GetProductPromotionsBySku";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Sku", sku);

            var dataTable = new DataTable();
            using (var reader = await command.ExecuteReaderAsync())
            {
                dataTable.Load(reader); // Synchronously loads all rows into DataTable
            }

            var results = DataTableHelper.DataTableToList<ProductPromotion>(dataTable);
            return results.Cast<IProductPromotion>().ToList();
        }

        public async Task<List<IProductPromotion>> GetProductPromotionsByPromoNoAsync(string promoNo)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_GetProductPromotionsByPromoNo";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PromoNo", promoNo);

            var dataTable = new DataTable();
            using (var reader = await command.ExecuteReaderAsync())
            {
                dataTable.Load(reader); // Synchronously loads all rows into DataTable
            }

            var results = DataTableHelper.DataTableToList<ProductPromotion>(dataTable);
            return results.Cast<IProductPromotion>().ToList();
        }

        public async Task<List<IPromotionDto>> SearchPromotionsAsync(IPromoSearchParams promoSearchParams)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_SearchPromotions";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@promoNo", promoSearchParams.PromoNo);
            command.Parameters.AddWithValue("@fromDate", promoSearchParams.FromDate);
            command.Parameters.AddWithValue("@toDate", promoSearchParams.ToDate);
            command.Parameters.AddWithValue("@storecode", promoSearchParams.StoreCode);
            command.Parameters.AddWithValue("@promotionType", promoSearchParams.PromoType);
            command.Parameters.AddWithValue("@promoDescription", promoSearchParams.PromoDescription);
            command.Parameters.AddWithValue("@status", promoSearchParams.Status);

            var dataTable = new DataTable();
            using (var reader = await command.ExecuteReaderAsync())
            {
                dataTable.Load(reader); // Synchronously loads all rows into DataTable
            }
            var results = DataTableHelper.DataTableToList<PromotionDto>(dataTable);
            foreach (var item in results)
            {
                item.StoreCodes = item.StoreCodeList.Split(",").ToList();
            }
            return results.Cast<IPromotionDto>().ToList();
        }

        public async Task CreatePromotionAsync(IPromotionRequest promotionCreateRequest)
        {
            var promoCreateRequest = (PromotionRequest)promotionCreateRequest;
            var dateNow = DateTime.Now;
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_CreatePromotion";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@promodate", dateNow);
            command.Parameters.AddWithValue("@promotype", promoCreateRequest.PromoType);
            command.Parameters.AddWithValue("@fromDate", promoCreateRequest.FromDate);
            command.Parameters.AddWithValue("@toDate", promoCreateRequest.ToDate);
            command.Parameters.AddWithValue("@sumqty", promoCreateRequest.SumQty);
            command.Parameters.AddWithValue("@sumamt", promoCreateRequest.SumAmt);
            command.Parameters.AddWithValue("@sumadd", promoCreateRequest.SumAdd);
            command.Parameters.AddWithValue("@dc", promoCreateRequest.PromoRate);
            command.Parameters.AddWithValue("@description", promoCreateRequest.PromoName);
            command.Parameters.AddWithValue("@finalSale", "");
            command.Parameters.AddWithValue("@status", false);
            command.Parameters.AddWithValue("@writedate", dateNow);
            command.Parameters.AddWithValue("@writeuser", promoCreateRequest.CurrentUser);
            command.Parameters.AddWithValue("@lastupdate", dateNow);
            command.Parameters.AddWithValue("@lastuser", promoCreateRequest.CurrentUser);


            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ItmPromoDetail",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_ItmPromoDetail",
                Value = DataTableHelper.ToDataTable(promoCreateRequest.PromotionItems)
            });

            DataTable promoTypeRule = new DataTable();
            promoTypeRule.Columns.Add("NameValue");
            promoTypeRule.Columns.Add("Value");
            DataTable promoRateRule = new DataTable();
            promoRateRule.Columns.Add("NameValue");
            promoRateRule.Columns.Add("Value");
            DataTable vendorRule = new DataTable();
            vendorRule.Columns.Add("NameValue");
            vendorRule.Columns.Add("Value");
            DataTable itemGroupRule = new DataTable();
            itemGroupRule.Columns.Add("NameValue");
            itemGroupRule.Columns.Add("Value");
            DataTable brandRule = new DataTable();
            brandRule.Columns.Add("NameValue");
            brandRule.Columns.Add("Value");
            DataTable retailPriceConditionRule = new DataTable();
            retailPriceConditionRule.Columns.Add("NameValue");
            retailPriceConditionRule.Columns.Add("Value");
            DataTable retailPriceRateRule = new DataTable();
            retailPriceRateRule.Columns.Add("NameValue");
            retailPriceRateRule.Columns.Add("Value");

            var ruleId = 0;
            foreach (var rule in promoCreateRequest.PromotionRules)
            {
                ruleId++;
                promoTypeRule.Rows.Add(new object[] { ruleId, rule.PromoType });
                promoRateRule.Rows.Add(new object[] { ruleId, rule.PromoRate });
                vendorRule.Rows.Add(new object[] { ruleId, rule.VendorCode });
                itemGroupRule.Rows.Add(new object[] { ruleId, rule.ItemGroup });
                brandRule.Rows.Add(new object[] { ruleId, rule.Brand });
                retailPriceConditionRule.Rows.Add(new object[] { ruleId, rule.RetailPriceCondition });
                retailPriceRateRule.Rows.Add(new object[] { ruleId, rule.RetailPriceRate });
            }


            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoTypeRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoTypeRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoRateRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoRateRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@VendorRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = vendorRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ItemGroupRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = itemGroupRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@BrandRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = brandRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@RetailPriceConditionRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = retailPriceConditionRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@RetailPriceRateRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = retailPriceRateRule
            });

            var dataSet = new DataSet();
            using (var reader = await command.ExecuteReaderAsync())
            {
                do
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    dataSet.Tables.Add(dt);
                } while (!reader.IsClosed && reader.NextResult());
            }

            //await command.ExecuteNonQueryAsync();
        }
        public async Task UpdatePromotionAsync(IPromotionRequest promotionRequest)
        {
            var promoRequest = (PromotionRequest)promotionRequest;
            var dateNow = DateTime.Now;
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_UpdatePromotion";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PromoNo", promoRequest.PromoNo);
            command.Parameters.AddWithValue("@promotype", promoRequest.PromoType);
            command.Parameters.AddWithValue("@fromDate", promoRequest.FromDate);
            command.Parameters.AddWithValue("@toDate", promoRequest.ToDate);
            command.Parameters.AddWithValue("@sumqty", promoRequest.SumQty);
            command.Parameters.AddWithValue("@sumamt", promoRequest.SumAmt);
            command.Parameters.AddWithValue("@sumadd", promoRequest.SumAdd);
            command.Parameters.AddWithValue("@dc", promoRequest.PromoRate);
            command.Parameters.AddWithValue("@description", promoRequest.PromoName);
            command.Parameters.AddWithValue("@finalSale", "");
            command.Parameters.AddWithValue("@status", promoRequest.PromoStatus);
            command.Parameters.AddWithValue("@lastupdate", dateNow);
            command.Parameters.AddWithValue("@lastuser", promoRequest.CurrentUser);

            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ItmPromoDetail",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_ItmPromoDetail",
                Value = DataTableHelper.ToDataTable(promoRequest.PromotionItems)
            });

            DataTable promoTypeRule = new DataTable();
            promoTypeRule.Columns.Add("NameValue");
            promoTypeRule.Columns.Add("Value");
            DataTable promoRule = new DataTable();
            promoRule.Columns.Add("NameValue");
            promoRule.Columns.Add("Value");
            DataTable promoRateRule = new DataTable();
            promoRateRule.Columns.Add("NameValue");
            promoRateRule.Columns.Add("Value");
            DataTable vendorRule = new DataTable();
            vendorRule.Columns.Add("NameValue");
            vendorRule.Columns.Add("Value");
            DataTable itemGroupRule = new DataTable();
            itemGroupRule.Columns.Add("NameValue");
            itemGroupRule.Columns.Add("Value");
            DataTable brandRule = new DataTable();
            brandRule.Columns.Add("NameValue");
            brandRule.Columns.Add("Value");
            DataTable retailPriceConditionRule = new DataTable();
            retailPriceConditionRule.Columns.Add("NameValue");
            retailPriceConditionRule.Columns.Add("Value");
            DataTable retailPriceRateRule = new DataTable();
            retailPriceRateRule.Columns.Add("NameValue");
            retailPriceRateRule.Columns.Add("Value");

            var ruleId = 0;
            foreach (var rule in promoRequest.PromotionRules)
            {
                ruleId = rule.PromoRuleId;
                if (ruleId == 0)
                    ruleId++;
                promoTypeRule.Rows.Add(new object[] { ruleId, rule.PromoType });
                promoRule.Rows.Add(new object[] { ruleId, promoRequest.PromoNo });
                promoRateRule.Rows.Add(new object[] { ruleId, rule.PromoRate });
                vendorRule.Rows.Add(new object[] { ruleId, rule.VendorCode });
                itemGroupRule.Rows.Add(new object[] { ruleId, rule.ItemGroup });
                brandRule.Rows.Add(new object[] { ruleId, rule.Brand });
                retailPriceConditionRule.Rows.Add(new object[] { ruleId, rule.RetailPriceCondition });
                retailPriceRateRule.Rows.Add(new object[] { ruleId, rule.RetailPriceRate });
            }


            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoTypeRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoTypeRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoRateRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoRateRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@VendorRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = vendorRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ItemGroupRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = itemGroupRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@BrandRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = brandRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@RetailPriceConditionRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = retailPriceConditionRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@RetailPriceRateRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = retailPriceRateRule
            });

            var dataSet = new DataSet();
            using (var reader = await command.ExecuteReaderAsync())
            {
                do
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    dataSet.Tables.Add(dt);
                } while (!reader.IsClosed && reader.NextResult());
            }

            //await command.ExecuteNonQueryAsync();
        }
        public async Task<IPromotionDto> GetByPromoNoAsync(string promono)
        {
            PromoSearchParams promoSearchParams = new PromoSearchParams();
            promoSearchParams.PromoNo = promono;

            var promoList = await SearchPromotionsAsync(promoSearchParams);
            var promo = promoList.FirstOrDefault();
            if (promo.PromoType == "P")
            {
                promo.DC = promo.DC * 100;
            }
            if (promo != null)
            {
                promo.PromotionRules = await GetPromoRulesByPromoNoAsync(promono);
            }
            return promo;
        }
        public async Task<List<IPromotionRuleDto>> GetPromoRulesByPromoNoAsync(string promono)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_GetPromoRules";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@promoNo", promono);

            var dataTable = new DataTable();
            using (var reader = await command.ExecuteReaderAsync())
            {
                dataTable.Load(reader); // Synchronously loads all rows into DataTable
            }
            var results = DataTableHelper.DataTableToList<PromotionRuleDto>(dataTable);
            foreach (var result in results)
            {
                if (result.PromoType == "P") { 
                    result.PromoRate = result.PromoRate * 100;
                }
            }
            return results.Cast<IPromotionRuleDto>().ToList();
        }
        public async Task TransferPromoAsync(IPromotionRequest promotionRequest)
        {
            var promoRequest = (PromotionRequest)promotionRequest;
            var dateNow = DateTime.Now;
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_TransferPromo";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PromoNo", promoRequest.PromoNo);
            command.Parameters.AddWithValue("@promodate", promoRequest.PromoDate.ToString("yyyyMMdd"));
            command.Parameters.AddWithValue("@promotype", promoRequest.PromoType);
            command.Parameters.AddWithValue("@fromDate", promoRequest.FromDate.ToString("yyyyMMdd"));
            command.Parameters.AddWithValue("@toDate", promoRequest.ToDate.ToString("yyyyMMdd"));
            command.Parameters.AddWithValue("@sumqty", promoRequest.SumQty);
            command.Parameters.AddWithValue("@sumamt", promoRequest.SumAmt);
            command.Parameters.AddWithValue("@sumadd", promoRequest.SumAdd);
            command.Parameters.AddWithValue("@dc", promoRequest.PromoRate);
            command.Parameters.AddWithValue("@description", promoRequest.PromoName);
            command.Parameters.AddWithValue("@finalSale", "");
            command.Parameters.AddWithValue("@status", promoRequest.PromoStatus);
            command.Parameters.AddWithValue("@lastupdate", dateNow);
            command.Parameters.AddWithValue("@lastuser", promoRequest.CurrentUser);

            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ItmPromoDetail",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_ItmPromoDetail",
                Value = DataTableHelper.ToDataTable(promoRequest.PromotionItems)
            });

            DataTable promoRule = new DataTable();
            promoRule.Columns.Add("NameValue");
            promoRule.Columns.Add("Value");
            DataTable promoTypeRule = new DataTable();
            promoTypeRule.Columns.Add("NameValue");
            promoTypeRule.Columns.Add("Value");
            DataTable promoRateRule = new DataTable();
            promoRateRule.Columns.Add("NameValue");
            promoRateRule.Columns.Add("Value");
            DataTable vendorRule = new DataTable();
            vendorRule.Columns.Add("NameValue");
            vendorRule.Columns.Add("Value");
            DataTable itemGroupRule = new DataTable();
            itemGroupRule.Columns.Add("NameValue");
            itemGroupRule.Columns.Add("Value");
            DataTable brandRule = new DataTable();
            brandRule.Columns.Add("NameValue");
            brandRule.Columns.Add("Value");
            DataTable retailPriceConditionRule = new DataTable();
            retailPriceConditionRule.Columns.Add("NameValue");
            retailPriceConditionRule.Columns.Add("Value");
            DataTable retailPriceRateRule = new DataTable();
            retailPriceRateRule.Columns.Add("NameValue");
            retailPriceRateRule.Columns.Add("Value");

            var ruleId = 0;
            foreach (var rule in promoRequest.PromotionRules)
            {
                ruleId = rule.PromoRuleId;
                if (ruleId == 0)
                    ruleId++;
                promoTypeRule.Rows.Add(new object[] { ruleId, rule.PromoType });
                promoRule.Rows.Add(new object[] { ruleId, promoRequest.PromoNo });
                promoRateRule.Rows.Add(new object[] { ruleId, rule.PromoRate });
                vendorRule.Rows.Add(new object[] { ruleId, rule.VendorCode });
                itemGroupRule.Rows.Add(new object[] { ruleId, rule.ItemGroup });
                brandRule.Rows.Add(new object[] { ruleId, rule.Brand });
                retailPriceConditionRule.Rows.Add(new object[] { ruleId, rule.RetailPriceCondition });
                retailPriceRateRule.Rows.Add(new object[] { ruleId, rule.RetailPriceRate });
            }


            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoTypeRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoTypeRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PromoRateRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = promoRateRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@VendorRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = vendorRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ItemGroupRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = itemGroupRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@BrandRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = brandRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@RetailPriceConditionRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = retailPriceConditionRule
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@RetailPriceRateRule",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = retailPriceRateRule
            });

            var dataSet = new DataSet();
            using (var reader = await command.ExecuteReaderAsync())
            {
                do
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    dataSet.Tables.Add(dt);
                } while (!reader.IsClosed && reader.NextResult());
            }
        }

        public async Task UpdatePromoStoreAsync(IPromoTransferRequest promoTransferRequest)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_UpdatePromoStores";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PromoNo", promoTransferRequest.PromoNo);

            DataTable dataTable = DataTableHelper.StringListParameter(promoTransferRequest.StoreCodes);

            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@StoreCodes",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_StringList",
                Value = dataTable
            });
            await command.ExecuteNonQueryAsync();
        }
    }
}
