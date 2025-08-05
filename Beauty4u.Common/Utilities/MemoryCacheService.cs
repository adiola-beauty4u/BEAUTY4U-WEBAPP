using Beauty4u.Interfaces.Common.Utilities;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Beauty4u.Common.Utilities
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;
        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T AddToCache<T>(T input, string cacheKey)
        {
            _cache.Set(cacheKey, JsonSerializer.Serialize(input), new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            });
            return input;
        }
        public bool TryGetFromCache<T>(string cacheKey, ref T output) where T : class
        {
            if (_cache.TryGetValue(cacheKey, out string jsonData))
            {
                output = JsonSerializer.Deserialize<T>(jsonData);
                return true;
            }

            return false;
        }
        public bool RemoveFromCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
            return true;
        }
    }
}
