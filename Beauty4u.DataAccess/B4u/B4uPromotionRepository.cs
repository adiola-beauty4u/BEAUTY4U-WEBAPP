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
            return results.Cast<IPromotionDto>().ToList();
        }

        public async Task CreatePromotions(IPromotionCreateRequest promotionCreateRequest)
        {
            var promoCreateRequest = (PromotionCreateRequest)promotionCreateRequest;
            var dateNow = DateTime.Now;
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_CreatePromotions";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@promodate", dateNow);
            command.Parameters.AddWithValue("@promotype", promoCreateRequest.PromoType);
            command.Parameters.AddWithValue("@fromDate", promoCreateRequest.FromDate);
            command.Parameters.AddWithValue("@toDate", promoCreateRequest.ToDate);
            command.Parameters.AddWithValue("@sumqty", 0);
            command.Parameters.AddWithValue("@sumamt", 0);
            command.Parameters.AddWithValue("@sumadd", 0);
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
    }
}
