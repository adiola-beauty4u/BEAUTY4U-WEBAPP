using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Auth;

namespace Beauty4u.Models.Api.Auth
{
    public class CurrentUserContext : ICurrentUserContext
    {
        public string? JwtToken { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public Dictionary<string, string> Claims { get; set; } = new();
    }
}
