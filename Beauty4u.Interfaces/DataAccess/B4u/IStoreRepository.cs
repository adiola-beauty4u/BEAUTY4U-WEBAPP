using Beauty4u.Interfaces.Dto.Stores;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IStoreRepository
    {
        Task<List<IStoreDto>> GetActiveStoresAsync();
        Task<List<IStoreDto>> GetAllStoresAsync();
    }
}