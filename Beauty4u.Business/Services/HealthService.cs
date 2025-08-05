using Beauty4u.Interfaces.Api.Health;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Business.Services
{
    public class HealthService : IHealthService
    {
        private readonly IStoreService _storeService;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IHealthApi _healthApi;
        public HealthService(IStoreService storeService, IConnectionRepository connectionRepository, IHealthApi healthApi)
        {
            _storeService = storeService;
            _connectionRepository = connectionRepository;
            _healthApi = healthApi;
        }
        public async Task<List<INameValue<bool>>> ServersUptimeCheckAsync()
        {
            var stores = await _storeService.GetActiveStoresAsync();
            var tasks = stores.Select(store => _connectionRepository.CheckConnection($"{store.Code} - {store.Name} ({store.StoreAbbr}) - Db", $"{store.RemoteIp}, {store.Port}"))
                                .ToList();

            tasks.AddRange(stores.Where(x => !string.IsNullOrWhiteSpace(x.ApiUrl))
                                   .Select(store => _healthApi.HealthCheck($"{store.Code} - {store.Name} ({store.StoreAbbr}) - Api", store.ApiUrl))
                                   .ToList());

            var results = await Task.WhenAll(tasks);

            return results.ToList();
        }
    }
}
