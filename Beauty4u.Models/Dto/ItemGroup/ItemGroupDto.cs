using Beauty4u.Interfaces.Dto.ItemGroup;

namespace Beauty4u.Models.Dto.ItemGroup
{
    public class ItemGroupDto : IItemGroupDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Status { get; set; }
        public string OldCode { get; set; } = null!;
        public int Orders { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteUser { get; set; } = null!;
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; } = null!;
        public string Level1Code { get; set; } = null!;
        public string Level2Code { get; set; } = null!;
        public List<ItemGroupDto> ChildItemGroups { get; set; }
    }
}
