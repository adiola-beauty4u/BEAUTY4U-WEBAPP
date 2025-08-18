using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Beauty4u.Common.Helpers;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.Promotions;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.Dto.Promotions;
using Beauty4u.Models.Dto.Stores;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
    }
}
