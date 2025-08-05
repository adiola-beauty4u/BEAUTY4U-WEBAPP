using Beauty4u.Interfaces.Api.Auth;

namespace Beauty4u.Interfaces.Services
{
    public interface ICurrentUserService
    {
        ICurrentUserContext? GetCurrentUser();
    }
}