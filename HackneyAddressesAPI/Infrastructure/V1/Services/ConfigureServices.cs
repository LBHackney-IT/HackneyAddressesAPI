using LBHAddressesAPI.Infrastructure.V1.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Infrastructure.V1.Services
{
    public static class ConfigureServices
    {
        public static void ConfigureAddressSearch(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<UseCases.V1.Addresses.IGetSingleAddressUseCase, UseCases.V1.Addresses.GetSingleAddressUseCase>();
            services.AddTransient<UseCases.V1.Addresses.ISearchAddressUseCase, UseCases.V1.Addresses.SearchAddressUseCase>();
            services.AddTransient<Gateways.V1.IAddressesGateway>(s => new Gateways.V1.AddressesGateway(connectionString));
        }
    }
}
