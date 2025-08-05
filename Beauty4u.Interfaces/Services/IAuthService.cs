using Beauty4u.Interfaces.Api.Auth;

namespace Beauty4u.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ITokenResponse?> LoginAsync(string username, string password);
        Task<ITokenResponse?> RefreshTokensAsync(IRefreshTokenRequest refreshTokenRequest);
        Task<string> ResetPasswordAsync(IResetPasswordRequest resetPasswordRequest);
    }
}