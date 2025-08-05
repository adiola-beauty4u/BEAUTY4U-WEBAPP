using Beauty4u.Interfaces.Api.Health;
using Beauty4u.Models.Common;
using System;

namespace Beauty4u.ApiAccess.Health
{
    public class HealthApi : IHealthApi
    {
        private readonly HttpClient _httpClient;
        readonly string healthEndpoint = "/health";

        public HealthApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<INameValue<bool>> HealthCheck(string storeCode, string baseAddress)
        {
            var response = await _httpClient.GetAsync($"{baseAddress}{healthEndpoint}");
            return new NameValue<bool>() { Name = storeCode, Value = response.IsSuccessStatusCode };
        }
    }
}
