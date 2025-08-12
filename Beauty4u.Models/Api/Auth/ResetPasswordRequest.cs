using Beauty4u.Interfaces.Api.Auth;

namespace Beauty4u.Models.Api.Auth
{
    public class ResetPasswordRequest : IResetPasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
