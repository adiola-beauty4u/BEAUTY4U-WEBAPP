using Beauty4u.ApiAccess.Health;
using Beauty4u.ApiAccess.Products;
using Beauty4u.Interfaces.Api.Health;
using Beauty4u.Interfaces.DataAccess.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Beauty4u.ApiAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiAccessDi(this IServiceCollection services)
        {
            services.AddScoped<IProductsApi, ProductsApi>();
            services.AddScoped<IHealthApi, HealthApi>();
            return services;
        }
    }
}
