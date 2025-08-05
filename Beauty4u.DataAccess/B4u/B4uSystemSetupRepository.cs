using AutoMapper;
using Beauty4u.Common.Helpers;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto;
using Beauty4u.Models.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Beauty4u.DataAccess.B4u
{
    public class B4uSystemSetupRepository : ISystemSetupRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public B4uSystemSetupRepository(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("B4uConnection")
            ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing");
            _mapper = mapper;
        }

        public async Task<List<ISystemSetupDto>> GetSystemSetupAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "[dbo].[usp_GetSystemSetup]";
            command.CommandType = CommandType.StoredProcedure;

            var dataTable = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            var results = DataTableHelper.DataTableToList<SystemSetupDto>(dataTable);
            return results.Cast<ISystemSetupDto>().ToList();
        }
    }
}
