using Beauty4u.Interfaces.Common.Utilities;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.Stores;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Dto.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Business.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger<StoreService> _logger;
        private readonly IMemoryCacheService _cache;
        public StoreService(IStoreRepository storeRepository, ILogger<StoreService> logger, IMemoryCacheService cache)
        {
            _storeRepository = storeRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<IStoreDto>> GetActiveStoresAsync()
        {
            try
            {
                string cacheKey = "allActiveStores";
                List<StoreDto> cachedStores = new List<StoreDto>();
                // Try to get from cache
                if (_cache.TryGetFromCache<List<StoreDto>>(cacheKey, ref cachedStores))
                {
                    return cachedStores.Cast<IStoreDto>().ToList();
                }
                
                // Get from repository
                var stores = await _storeRepository.GetActiveStoresAsync();

                // Store in cache
                _cache.AddToCache(stores.Cast<StoreDto>().ToList(), cacheKey);

                return stores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting active stores.");
                return new List<IStoreDto>(); // fallback in case of failure
            }
        }

        public async Task<List<IStoreDto>> GetAllStoresAsync()
        {
            try
            {
                string cacheKey = "allStores";
                List<StoreDto> cachedStores = new List<StoreDto>();
                // Try to get from cache
                if (_cache.TryGetFromCache<List<StoreDto>>(cacheKey, ref cachedStores))
                {
                    return cachedStores.Cast<IStoreDto>().ToList();
                }

                // Get from repository
                var stores = await _storeRepository.GetAllStoresAsync();

                // Store in cache
                _cache.AddToCache(stores.Cast<StoreDto>().ToList(), cacheKey);

                return stores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting active stores.");
                return new List<IStoreDto>(); // fallback in case of failure
            }
        }
    }
}
