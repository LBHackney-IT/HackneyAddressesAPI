using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Actions;
using HackneyAddressesAPI.DB;
using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace HackneyAddressesAPI
{
    public static class ServiceConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator, Validator>();
            services.AddTransient<IFormatter, Formatter>();

            services.AddTransient<IAddressesActions, AddressesActions>();
            services.AddTransient<IAddressDetailsMapper, AddressDetailsMapperOracle>();
            services.AddTransient<IFilterObjectBuilder, FilterObjectBuilder>();

            services.AddTransient<IDB_Helper, OracleHelper>();
            services.AddTransient<IQueryBuilder, AddressesQueryBuilderOracle>();

            services.AddScoped<IConfigReader, ConfigReader>();

            services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
        }
    }
}
