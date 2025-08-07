using AutoMapper;
using Beauty4u.Common.Helpers;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.DataAccess.B4u;
using Beauty4u.Models.Dto.Products;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Beauty4u.DataAccess.B4u
{
    public class B4uProductRepository : IProductRepository
    {
        //private readonly B4uDbContext _b4uDbContext;
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        public B4uProductRepository(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("B4uConnection")
            ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing");
            _mapper = mapper;
        }
        public async Task<List<IBulkProductResultDto>> BulkProductRegisterAsync(IBulkProductParams bulkProductRequest)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_BulkProductRegistration";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@VendorId", bulkProductRequest.VendorId);
            command.Parameters.AddWithValue("@UserCode", bulkProductRequest.UserCode);
            command.Parameters.AddWithValue("@VendorCode", bulkProductRequest.VendorCode);
            command.Parameters.AddWithValue("@VendorName", bulkProductRequest.VendorName);
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@BulkProducts",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_BulkProducts",
                Value = bulkProductRequest.BulkProducts
            });

            var dataSet = new DataSet();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            return DataTableHelper.DataTableToList<BulkProductResultDto>(dataSet.Tables[dataSet.Tables.Count - 1]).Cast<IBulkProductResultDto>().ToList();
        }

        public async Task BulkProductUpdateAsync(IBulkProductParams bulkProductRequest)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_BulkProductUpdate";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@VendorId", bulkProductRequest.VendorId);
            command.Parameters.AddWithValue("@UserCode", bulkProductRequest.UserCode);
            command.Parameters.AddWithValue("@VendorCode", bulkProductRequest.VendorCode);
            command.Parameters.AddWithValue("@VendorName", bulkProductRequest.VendorName);
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@BulkProducts",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_BulkProducts",
                Value = bulkProductRequest.BulkProducts
            });

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<ISearchProductResult>> ProductTransferSearchAsync(DateOnly startDate, DateOnly endDate)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_ProductTransferSearch";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@DTSTART", SqlDbType.Date) { Value = startDate.ToShortDateString() });
            command.Parameters.Add(new SqlParameter("@DTEND", SqlDbType.Date) { Value = endDate.AddDays(1).ToShortDateString() });

            using var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);

            return DataTableHelper.DataTableToList<SearchProductResult>(table).Cast<ISearchProductResult>().ToList();
        }

        public async Task<List<IUPCValidateResult>> ValidateUPCListAsync(DataTable upcList)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_ValidateUPCList";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@StringList",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_StringList",
                Value = upcList
            });

            var dataTable = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            var results = DataTableHelper.DataTableToList<UPCValidateResult>(dataTable);
            return results.Cast<IUPCValidateResult>().ToList();
        }

        public async Task<List<IBulkProductUpdatePreviewResult>> BulkUpdatePreview(DataTable bulkUpdateList)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_BulkUpdatePreview";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@BulkUpdateList",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_BulkProducts",
                Value = bulkUpdateList
            });

            var dataTable = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            var results = DataTableHelper.DataTableToList<BulkProductUpdatePreviewResult>(dataTable);
            return results.Cast<IBulkProductUpdatePreviewResult>().ToList();
        }

        public async Task<List<ISearchProductResult>> ProductSearchByUPCListAsync(DataTable upcList)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_ProductSearchByUPCList";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@StringList",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_StringList",
                Value = upcList
            });

            using var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);

            return DataTableHelper.DataTableToList<SearchProductResult>(table).ToList<ISearchProductResult>();
        }

        public async Task<List<IBulkProductUpdatePreviewResult>> TransferProductsAsync(ITransferProductParams transferProductParams)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_TransferProducts";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@UserCode", transferProductParams.UserCode));
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ProductTransferDetails",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_ProductTransferDetails",
                Value = transferProductParams.ProductTransferDetails
            });

            var dataTable = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            var results = DataTableHelper.DataTableToList<BulkProductUpdatePreviewResult>(dataTable);
            return results.Cast<IBulkProductUpdatePreviewResult>().ToList();
        }

        public async Task LogBulkProductRequestAsync(IBulkProductRequestParams bulkProductRequestParams)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_LogBulkProductRequest";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@VendorId", bulkProductRequestParams.VendorId);
            command.Parameters.AddWithValue("@UserCode", bulkProductRequestParams.UserCode);
            command.Parameters.AddWithValue("@VendorCode", bulkProductRequestParams.VendorCode);
            command.Parameters.AddWithValue("@VendorName", bulkProductRequestParams.VendorName);
            command.Parameters.AddWithValue("@StartTime", bulkProductRequestParams.StartTime);
            command.Parameters.AddWithValue("@EndTime", bulkProductRequestParams.EndTime);
            command.Parameters.AddWithValue("@IsScheduled", bulkProductRequestParams.IsScheduled);
            command.Parameters.AddWithValue("@UploadType", bulkProductRequestParams.UploadType);
            command.Parameters.AddWithValue("@IsSuccessful", bulkProductRequestParams.IsSuccessful);
            command.Parameters.AddWithValue("@Result", bulkProductRequestParams.Result);
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@BulkProducts",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_BulkProducts",
                Value = bulkProductRequestParams.BulkProducts
            });

            await command.ExecuteNonQueryAsync();
        }

        public async Task LogProductTransfersAsync(ILogProductTransfersParam logProductTransfersParam)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_LogProductTransfers";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserCode", logProductTransfersParam.UserCode);
            command.Parameters.AddWithValue("@StoreCode", logProductTransfersParam.StoreCode);
            command.Parameters.AddWithValue("@RequestDate", logProductTransfersParam.RequestDate);
            command.Parameters.AddWithValue("@StartTime", logProductTransfersParam.StartTime);
            command.Parameters.AddWithValue("@EndTime", logProductTransfersParam.EndTime);
            command.Parameters.AddWithValue("@IsSuccessful", logProductTransfersParam.IsSuccessful);
            command.Parameters.AddWithValue("@IsScheduled", logProductTransfersParam.IsScheduled);
            command.Parameters.AddWithValue("@Result", logProductTransfersParam.Result);
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@IsNew",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = logProductTransfersParam.IsNew
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@CurrentRetailPrice",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = logProductTransfersParam.CurrentRetailPrice
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@NewRetailPrice",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = logProductTransfersParam.NewRetailPrice
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@CurrentCost",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = logProductTransfersParam.CurrentCost
            });
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = "@NewCost",
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.udt_NameValueString",
                Value = logProductTransfersParam.NewCost
            });

            await command.ExecuteNonQueryAsync();
        }

        public async Task SearchProducts()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
        }
    }
}