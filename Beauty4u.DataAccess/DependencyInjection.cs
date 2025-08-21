using Beauty4u.Data;
using Beauty4u.DataAccess.B4u;
using Beauty4u.Models.DataAccess.B4u;
using Beauty4u.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Models.Dto;

namespace Beauty4u.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessDi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddModelsDi();
            services.AddDataDI(configuration);


            //services.AddTransient<Func<string, IProductRepository>>(sp => key =>
            //{
            //    return key.ToLower() switch
            //    {
            //        "v1" => sp.GetRequiredService<B4uProductRepository>(),
            //        _ => throw new ArgumentException("Invalid notifier key")
            //    };
            //});

            //services.Scan(scan => scan
            //            .FromAssemblyOf<IProductRepository>() // or Assembly.GetExecutingAssembly()
            //            .AddClasses()
            //            .AsImplementedInterfaces()   // registers as INotifier
            //            .WithScopedLifetime());      // or WithTransientLifetime()
            services.AddScoped<IProductRepository, B4uProductRepository>();
            services.AddScoped<IVendorRepository, B4uVendorRepository>();
            services.AddScoped<IUserRepository, B4uUserRepository>();
            services.AddScoped<IStoreRepository, B4uStoreRepository>();
            services.AddScoped<ISystemSetupRepository, B4uSystemSetupRepository>();
            services.AddScoped<IConnectionRepository, B4uConnectionRepository>();
            services.AddScoped<IItemGroupRepository, B4uItemGroupRepository>();
            services.AddScoped<IPromotionRepository, B4uPromotionRepository>();
            services.AddScoped<IBrandRepository, B4uBrandRepository>();
            return services;
        }
    }
}
