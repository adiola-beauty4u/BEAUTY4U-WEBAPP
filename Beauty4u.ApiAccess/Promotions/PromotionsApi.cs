using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.DataAccess.Api;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto.Products;

namespace Beauty4u.ApiAccess.Promotions
{
    public class PromotionsApi : IPromotionsApi
    {
        private readonly HttpClient _httpClient;
        readonly string promotionsEndpoint = "/promotions";

        public PromotionsApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> SeachBySkuInApiAsync<T>(string baseAddress, string jwtToken, string sku)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await _httpClient.GetAsync($"{baseAddress}{promotionsEndpoint}/search-by-sku?sku={sku}");

                response.EnsureSuccessStatusCode();
                var output = await response.Content.ReadFromJsonAsync<T>();

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
