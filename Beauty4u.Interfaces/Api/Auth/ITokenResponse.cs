namespace Beauty4u.Interfaces.Api.Auth
{
    public interface ITokenResponse
    {
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
    }
}