using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using Beauty4u.Interfaces.Dto.Products;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Beauty4u.Interfaces.DataAccess.Api;
using Beauty4u.Models.Dto.Products;
using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Models.Api.Products;

namespace Beauty4u.ApiAccess.Products
{
    public class ProductsApi : IProductsApi
    {
        private readonly HttpClient _httpClient;
        readonly string productEndpoint = "/products";
        public ProductsApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ISearchProductResult>> SearchProductFromApiAsync(string baseAddress, string jwtToken, List<string> upcList)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.PostAsJsonAsync($"{baseAddress}{productEndpoint}/search-by-upc", upcList);

            response.EnsureSuccessStatusCode();
            var output = await response.Content.ReadFromJsonAsync<List<SearchProductResult>>();

            return output.ToList<ISearchProductResult>();

        }
        public async Task<T> TransferProductsToApiAsync<T>(string baseAddress, string jwtToken, List<ISearchProductResult> transferList)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.PostAsJsonAsync($"{baseAddress}{productEndpoint}/transfer", transferList);

            response.EnsureSuccessStatusCode();
            var output = await response.Content.ReadFromJsonAsync<T>();

            return output;

        }
    }
}
