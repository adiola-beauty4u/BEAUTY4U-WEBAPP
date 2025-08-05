using Beauty4u.Interfaces.Dto.ItemGroup;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IItemGroupRepository
    {
        Task<List<IItemGroupDto>> GetActiveItemGroupsAsync();
    }
}