using Beauty4u.Interfaces.Api.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api.Auth
{
    public class RefreshTokenRequest : IRefreshTokenRequest
    {
        public string UserCode { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
