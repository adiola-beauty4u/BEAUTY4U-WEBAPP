using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api
{
    public class DateSearchRequest
    {
        public DateOnly DateStart { get; set; }
        public DateOnly DateEnd { get; set; }
    }
}
