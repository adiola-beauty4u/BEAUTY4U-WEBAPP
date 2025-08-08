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

            var data = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(data);

            var results = DataTableHelper.DataTableToList<ProductPromotion>(data);
            return results.Cast<IProductPromotion>().ToList();
        }
    }
}
