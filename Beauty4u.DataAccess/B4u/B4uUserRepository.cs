using AutoMapper;
using Beauty4u.Common.Helpers;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.Users;
using Beauty4u.Models.Dto.Users;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Beauty4u.DataAccess.B4u
{
    public class B4uUserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        public B4uUserRepository(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("B4uConnection")
            ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing");
            _mapper = mapper;
        }

        public async Task<IUserDto> GetUserLoginByUsernameAsync(string userName)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_GetUserLoginById";
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 300;

            command.Parameters.AddWithValue("@UserId", userName);

            var dataTable = new DataTable();
            using (var reader = await command.ExecuteReaderAsync())
            {
                dataTable.Load(reader); // Synchronously loads all rows into DataTable
            }

            var users = DataTableHelper.DataTableToList<UserDto>(dataTable);

            //var users = dataTable.ToList<UserDto>();

            if (users.Count > 0)
            {
                return users[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<IUserDto> GetUserLoginByUsercodeAsync(string userName)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_GetUserLoginByUsercode";
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 300;

            command.Parameters.AddWithValue("@UserCode", userName);

            var dataTable = new DataTable();
            using (var reader = await command.ExecuteReaderAsync())
            {
                dataTable.Load(reader); // Synchronously loads all rows into DataTable
            }

            var users = DataTableHelper.DataTableToList<UserDto>(dataTable);

            //var users = dataTable.ToList<UserDto>();

            if (users.Count > 0)
            {
                return users[0];
            }
            else
            {
                return null;
            }
        }

        public async Task UserUpdateRefreshTokenAsync(IUserDto user)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "dbo.usp_UserUpdateRefreshToken";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 300;

                command.Parameters.AddWithValue("@UserCode", user.UserCode);
                command.Parameters.AddWithValue("@RefreshToken", user.RefreshToken);
                command.Parameters.AddWithValue("@RefreshTokenExpiry", user.RefreshTokenExpiry);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task UpdateUserPasswordHashAsync(string userCode, string newPassword)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "dbo.usp_UpdateUserPasswordHash";
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 300;

            command.Parameters.AddWithValue("@UserCode", userCode);
            command.Parameters.AddWithValue("@PasswordHash", newPassword);

            await command.ExecuteNonQueryAsync();
        }
    }
}
