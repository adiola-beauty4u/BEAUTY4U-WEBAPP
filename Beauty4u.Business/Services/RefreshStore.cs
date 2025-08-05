using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Business.Services
{
    public static class RefreshStore
    {
        public static Dictionary<string, string> UserRefreshTokens = new(); // [username] = refreshToken
    }
}
