using Beauty4u.Interfaces.Dto.Stores;

namespace Beauty4u.Interfaces.Services
{
    public interface IStoreService
    {
        Task<List<IStoreDto>> GetActiveStoresAsync();
        Task<List<IStoreDto>> GetAllStoresAsync();
    }
}