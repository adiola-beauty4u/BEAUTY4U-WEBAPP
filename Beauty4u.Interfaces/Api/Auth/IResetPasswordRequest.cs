namespace Beauty4u.Interfaces.Api.Auth
{
    public interface IResetPasswordRequest
    {
        string CurrentPassword { get; set; }
        string NewPassword { get; set; }
    }
}