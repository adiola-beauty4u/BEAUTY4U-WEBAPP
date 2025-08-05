using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Models.Dto
{
    public class SystemSetupDto : ISystemSetupDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string AltValue { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
