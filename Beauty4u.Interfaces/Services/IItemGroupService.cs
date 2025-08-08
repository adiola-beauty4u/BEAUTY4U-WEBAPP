using Beauty4u.Interfaces.Dto.ItemGroup;

namespace Beauty4u.Interfaces.Services
{
    public interface IItemGroupService
    {
        Task<Dictionary<string, IItemGroupDto>> GetActiveItemGroupsAsync();
        Task<List<IItemGroupDto>> GetGroupedItemGroupsAsync();
    }
}