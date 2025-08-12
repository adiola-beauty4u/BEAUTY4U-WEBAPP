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
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string AltValue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
