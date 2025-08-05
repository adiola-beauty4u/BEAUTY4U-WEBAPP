using Beauty4u.ApiAccess;
using Beauty4u.Business.Services;
using Beauty4u.Common;
using Beauty4u.DataAccess;
using Beauty4u.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCommonDi(configuration);
            services.AddDataAccessDi(configuration);
            services.AddApiAccessDi();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IDataValidationService, DataValidationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ISystemSetupService, SystemSetupService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IHealthService, HealthService>();
            services.AddScoped<IItemGroupService, ItemGroupService>();
            return services;
        }
    }
}
