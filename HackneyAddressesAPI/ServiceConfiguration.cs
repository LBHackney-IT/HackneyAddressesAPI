using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.Actions;
using LBHAddressesAPI.DB;
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
            services.AddTransient<IValidator, Validator>();
            services.AddTransient<IFormatter, Formatter>();
            
            services.AddTransient<IStreetsActions, StreetsActions>();
            
            services.AddTransient<IFilterObjectBuilder, FilterObjectBuilder>();

            services.AddTransient<IDB_Helper, OracleHelper>();
            services.AddTransient<IQueryBuilder, QueryBuilderOracle>();

            services.AddScoped<IConfigReader, ConfigReader>();

            services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        }
    }
}
