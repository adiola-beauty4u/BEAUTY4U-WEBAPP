using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Models.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.DataAccess.B4u
{
    public class B4uConnectionRepository : IConnectionRepository
    {
        private readonly IConfiguration _config;
        public B4uConnectionRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<INameValue<bool>> CheckConnection(string storeCode, string serverName)
        {
            try
            {
                string connectionString = $"Data Source={serverName};Initial Catalog=master;Persist Security Info=True;User ID={_config["DbConnectionAccess:User"]};Password={_config["DbConnectionAccess:Password"]};Connection Timeout=300;TrustServerCertificate=True";
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return new NameValue<bool>() { Name = storeCode, Value = true };
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return new NameValue<bool>() { Name = storeCode, Value = false };
            }
        }
    }
}
