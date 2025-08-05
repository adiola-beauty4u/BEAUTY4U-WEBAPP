using Beauty4u.Common.Helpers;
using Beauty4u.Common.Utilities;
using Beauty4u.Interfaces.Common.Utilities;
using Beauty4u.Models.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beauty4u.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonDi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileReadHelper, FileReadHelper>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();
            return services;
        }
    }
}
