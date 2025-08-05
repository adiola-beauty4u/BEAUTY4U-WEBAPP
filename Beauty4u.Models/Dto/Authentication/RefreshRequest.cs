using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Authentication
{
    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
