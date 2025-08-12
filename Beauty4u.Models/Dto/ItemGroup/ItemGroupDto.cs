using Beauty4u.Interfaces.Dto.ItemGroup;

namespace Beauty4u.Models.Dto.ItemGroup
{
    public class ItemGroupDto : IItemGroupDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string OldCode { get; set; } = string.Empty;
        public int Orders { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteUser { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; } = string.Empty;
        public string Level1Code { get; set; } = string.Empty;
        public string Level2Code { get; set; } = string.Empty;
        public List<ItemGroupDto> ChildItemGroups { get; set; }
    }
}
