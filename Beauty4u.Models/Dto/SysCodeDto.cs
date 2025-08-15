using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Models.Dto
{
    public class SysCodeDto : ISysCodeDto
    {
        public string Class { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        public int Orders { get; set; }
    }
}
