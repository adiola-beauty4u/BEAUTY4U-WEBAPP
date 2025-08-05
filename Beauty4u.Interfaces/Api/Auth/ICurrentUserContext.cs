namespace Beauty4u.Interfaces.Api.Auth
{
    public interface ICurrentUserContext
    {
        Dictionary<string, string> Claims { get; set; }
        string? Email { get; set; }
        string? JwtToken { get; set; }
        string? Role { get; set; }
        string? UserId { get; set; }
    }
}