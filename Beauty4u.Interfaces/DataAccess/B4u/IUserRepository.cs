using Beauty4u.Interfaces.Dto.Users;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IUserRepository
    {
        Task<IUserDto> GetUserLoginByUsercodeAsync(string userName);
        Task<IUserDto> GetUserLoginByUsernameAsync(string userName);
        Task UpdateUserPasswordHashAsync(string userCode, string newPassword);
        Task UserUpdateRefreshTokenAsync(IUserDto user);
    }
}