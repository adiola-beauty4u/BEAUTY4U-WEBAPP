namespace Beauty4u.Interfaces.Dto.ItemGroup
{
    public interface IItemGroupDto
    {
        string Code { get; set; }
        string Description { get; set; }
        DateTime LastUpdate { get; set; }
        string LastUser { get; set; }
        string Name { get; set; }
        string OldCode { get; set; }
        int Orders { get; set; }
        bool Status { get; set; }
        DateTime WriteDate { get; set; }
        string WriteUser { get; set; }
        string Level1Code { get; set; }
        string Level2Code { get; set; }
    }
}