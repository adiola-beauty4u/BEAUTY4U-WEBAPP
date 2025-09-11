using Beauty4u.Interfaces.Api.Auth;
using Beauty4u.Interfaces.Dto.Users;

namespace Beauty4u.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ITokenResponse> CreateToken(IUserDto user);
        Task<ITokenResponse?> LoginAsync(string username, string password);
        Task<ITokenResponse?> RefreshTokensAsync(IRefreshTokenRequest refreshTokenRequest);
        Task<string> ResetPasswordAsync(IResetPasswordRequest resetPasswordRequest);
    }
}