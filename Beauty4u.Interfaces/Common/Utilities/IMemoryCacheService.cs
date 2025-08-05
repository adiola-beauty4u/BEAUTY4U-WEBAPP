namespace Beauty4u.Interfaces.Common.Utilities
{
    public interface IMemoryCacheService
    {
        T AddToCache<T>(T input, string cacheKey);
        bool RemoveFromCache(string cacheKey);
        bool TryGetFromCache<T>(string cacheKey, ref T output) where T : class;
    }
}