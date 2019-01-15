using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient<UseCases.V1.Addresses.IGetAddressUseCase, UseCases.V1.Addresses.GetAddressUseCase>();
            services.AddTransient<Gateways.V1.IAddressesGateway>(s => new Gateways.V1.AddressesGateway(connectionString));
        }
    }
}
