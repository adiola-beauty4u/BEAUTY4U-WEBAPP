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
    public static class SchedulerDependencyInjection
    {
        public static IServiceCollection AddSchedulerDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCommonDi(configuration);
            services.AddDataAccessDi(configuration);
            services.AddApiAccessDi();

            services.AddScoped<IScheduledJobService, ScheduledJobService>();
            return services;
        }
    }
}
