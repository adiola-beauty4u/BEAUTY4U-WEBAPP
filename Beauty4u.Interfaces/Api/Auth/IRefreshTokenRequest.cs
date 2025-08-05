namespace Beauty4u.Interfaces.Api.Auth
{
    public interface IRefreshTokenRequest
    {
        string RefreshToken { get; set; }
        string UserCode { get; set; }
    }
}