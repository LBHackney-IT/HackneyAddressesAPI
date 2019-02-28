using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.Helpers;
using LBHAddressesAPI.Interfaces;
using LBHAddressesAPI.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace LBHAddressesAPI
{
    public static class ServiceConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
                       

            //services.AddScoped<IConfigReader, ConfigReader>();

            services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        }
    }
}
