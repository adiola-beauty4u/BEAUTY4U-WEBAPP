using AutoMapper;
using Beauty4u.Common.Helpers;
using Beauty4u.Data.Context;
using Beauty4u.Interfaces.Dto.Stores;
using Beauty4u.Models.Dto;
using Beauty4u.Models.Dto.Stores;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Beauty4u.DataAccess.B4u
{
    public class B4uVendorRepository : IVendorRepository
    {
        private readonly B4uDbContext _b4uDbContext;
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        public B4uVendorRepository(IConfiguration configuration, IMapper mapper, B4uDbContext b4uDbContext)
        {
            _connectionString = configuration.GetConnectionString("B4uConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing");
            _mapper = mapper;
            _b4uDbContext = b4uDbContext;
        }

        public async Task<List<IVendorDto>> GetVendorsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "[dbo].[usp_GetActiveVendors]";
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 300;

            var dataTable = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            var results = DataTableHelper.DataTableToList<VendorDto>(dataTable);
            return results.Cast<IVendorDto>().ToList();
        }
    }
}
