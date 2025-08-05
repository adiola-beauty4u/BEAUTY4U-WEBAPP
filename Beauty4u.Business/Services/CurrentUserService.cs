using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Beauty4u.Models.Api.Auth;
using Beauty4u.Interfaces.Api.Auth;
using Beauty4u.Interfaces.Services;

namespace Beauty4u.Business.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ICurrentUserContext? GetCurrentUser()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null || context.User.Identity?.IsAuthenticated != true)
                return null;

            var user = context.User;
            var jwt = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrWhiteSpace(jwt) && jwt.StartsWith("Bearer "))
                jwt = jwt["Bearer ".Length..];

            var claims = user.Claims.ToDictionary(c => c.Type, c => c.Value);

            return new CurrentUserContext
            {
                JwtToken = jwt,
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                Role = user.FindFirst(ClaimTypes.Role)?.Value,
                Claims = claims
            };
        }
    }
}
