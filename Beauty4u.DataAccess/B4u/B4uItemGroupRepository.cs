using Beauty4u.Common.Helpers;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.ItemGroup;
using Beauty4u.Interfaces.Dto.Stores;
using Beauty4u.Models.Common;
using Beauty4u.Models.Dto.ItemGroup;
using Beauty4u.Models.Dto.Stores;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.DataAccess.B4u
{
    public class B4uItemGroupRepository : IItemGroupRepository
    {
        private readonly string _connectionString;
        public B4uItemGroupRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("B4uConnection")
           ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing");
        }

        public async Task<List<IItemGroupDto>> GetActiveItemGroupsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "[dbo].[usp_GetActiveItemGroups]";
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 300;

            var dataTable = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            var results = DataTableHelper.DataTableToList<ItemGroupDto>(dataTable);
            return results.Cast<IItemGroupDto>().ToList();
        }
    }
}
